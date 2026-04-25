import { computed, readonly, ref } from "vue";
import { buildBackendUrl } from "@/api/buildBackendUrl";

const ACCESS_TOKEN_STORAGE_KEY = "auth.access_token";
const TOKEN_EXPIRY_SKEW_MS = 15_000;
const REFRESH_UNSUPPORTED_STATUSES = new Set([404, 405]);

type TokenResponse = {
  token?: string;
};

type RegisterRequest = {
  email: string;
  username: string;
  password: string;
};

const accessToken = ref<string | null>(loadStoredToken());
const refreshUnsupported = ref(false);
let refreshInFlight: Promise<boolean> | null = null;

function loadStoredToken(): string | null {
  if (typeof window === "undefined") {
    return null;
  }

  const token = window.localStorage.getItem(ACCESS_TOKEN_STORAGE_KEY);
  return token && token.length > 0 ? token : null;
}

function saveStoredToken(token: string | null): void {
  if (typeof window === "undefined") {
    return;
  }

  if (token === null) {
    window.localStorage.removeItem(ACCESS_TOKEN_STORAGE_KEY);
    return;
  }

  window.localStorage.setItem(ACCESS_TOKEN_STORAGE_KEY, token);
}

function setAccessToken(token: string | null): void {
  accessToken.value = token;
  saveStoredToken(token);
}

function decodeJwtPayload(token: string): Record<string, unknown> | null {
  const parts = token.split(".");
  const payloadPart = parts[1];
  if (parts.length < 2 || !payloadPart) {
    return null;
  }

  try {
    const base64 = payloadPart
      .replace(/-/g, "+")
      .replace(/_/g, "/")
      .padEnd(Math.ceil(payloadPart.length / 4) * 4, "=");
    const decoded = atob(base64);
    return JSON.parse(decoded) as Record<string, unknown>;
  } catch {
    return null;
  }
}

function getTokenExpiryMs(token: string): number | null {
  const payload = decodeJwtPayload(token);
  const exp = payload?.exp;
  if (typeof exp !== "number") {
    return null;
  }
  return exp * 1000;
}

function isAccessTokenExpired(token: string): boolean {
  const expiryMs = getTokenExpiryMs(token);
  if (expiryMs === null) {
    return false;
  }
  return Date.now() + TOKEN_EXPIRY_SKEW_MS >= expiryMs;
}

async function readErrorMessage(response: Response): Promise<string> {
  const contentType = response.headers.get("content-type") ?? "";

  if (contentType.includes("application/json")) {
    const body = await response.json().catch(() => null);
    if (typeof body === "string" && body.length > 0) {
      return body;
    }
    if (body && typeof body.message === "string" && body.message.length > 0) {
      return body.message;
    }
    if (body && typeof body.title === "string" && body.title.length > 0) {
      return body.title;
    }
  }

  const text = await response.text().catch(() => "");
  if (text.length > 0) {
    return text;
  }

  return `Request failed with status ${response.status}`;
}

async function login(username: string, password: string): Promise<void> {
  const response = await fetch(buildBackendUrl("/api/Auth/login"), {
    method: "POST",
    headers: {
      accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ username, password }),
  });

  if (!response.ok) {
    throw new Error(await readErrorMessage(response));
  }

  const body = (await response.json()) as TokenResponse;
  if (typeof body.token !== "string" || body.token.length === 0) {
    throw new Error("Login failed because no access token was returned.");
  }

  setAccessToken(body.token);
}

async function register(email: string, username: string, password: string): Promise<void> {
  const registerRequest: RegisterRequest = {
    email,
    username,
    password,
  };

  const response = await fetch(buildBackendUrl("/api/Auth/register"), {
    method: "POST",
    headers: {
      accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify(registerRequest),
  });

  if (!response.ok) {
    throw new Error(await readErrorMessage(response));
  }

  await login(username, password);
}

function logout(): void {
  setAccessToken(null);
}

function redirectToLogin(): void {
  if (typeof window === "undefined") {
    return;
  }

  if (window.location.pathname === "/login") {
    return;
  }

  const redirectPath = `${window.location.pathname}${window.location.search}${window.location.hash}`;
  const nextPath = `/login?redirect=${encodeURIComponent(redirectPath)}`;
  window.location.assign(nextPath);
}

async function tryRefresh(): Promise<boolean> {
  if (refreshUnsupported.value) {
    return false;
  }

  if (refreshInFlight) {
    return refreshInFlight;
  }

  refreshInFlight = (async () => {
    const response = await fetch(buildBackendUrl("/api/Auth/refresh"), {
      method: "POST",
      headers: {
        accept: "application/json",
      },
      credentials: "include",
    });

    if (REFRESH_UNSUPPORTED_STATUSES.has(response.status)) {
      refreshUnsupported.value = true;
      return false;
    }

    if (!response.ok) {
      return false;
    }

    const body = (await response.json().catch(() => null)) as TokenResponse | null;
    if (!body || typeof body.token !== "string" || body.token.length === 0) {
      return false;
    }

    setAccessToken(body.token);
    return true;
  })()
    .catch(() => false)
    .finally(() => {
      refreshInFlight = null;
    });

  return refreshInFlight;
}

async function ensureSession(): Promise<boolean> {
  const token = accessToken.value;
  if (!token) {
    return false;
  }

  if (!isAccessTokenExpired(token)) {
    return true;
  }

  const refreshed = await tryRefresh();
  if (!refreshed) {
    logout();
    return false;
  }

  return true;
}

function getAccessToken(): string | null {
  return accessToken.value;
}

const isAuthenticated = computed<boolean>(() => {
  const token = accessToken.value;
  return token !== null && !isAccessTokenExpired(token);
});

const username = computed<string | null>(() => {
  const token = accessToken.value;
  if (!token) {
    return null;
  }

  const payload = decodeJwtPayload(token);
  const value =
    payload?.unique_name ??
    payload?.name ??
    payload?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

  if (typeof value !== "string" || value.length === 0) {
    return null;
  }

  return value;
});

export async function authFetch(input: RequestInfo | URL, init: RequestInit = {}): Promise<Response> {
  const headers = new Headers(init.headers);
  const token = getAccessToken();
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(input, {
    ...init,
    headers,
  });

  if (response.status !== 401) {
    return response;
  }

  const refreshed = await tryRefresh();
  if (!refreshed) {
    logout();
    redirectToLogin();
    return response;
  }

  const retryHeaders = new Headers(init.headers);
  const nextToken = getAccessToken();
  if (nextToken) {
    retryHeaders.set("Authorization", `Bearer ${nextToken}`);
  }

  const retryResponse = await fetch(input, {
    ...init,
    headers: retryHeaders,
  });

  if (retryResponse.status === 401) {
    logout();
    redirectToLogin();
  }

  return retryResponse;
}

export function useAuth() {
  return {
    accessToken: readonly(accessToken),
    isAuthenticated,
    username,
    login,
    register,
    logout,
    tryRefresh,
    ensureSession,
    getAccessToken,
  };
}
