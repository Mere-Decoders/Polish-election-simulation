const DEFAULT_BACKEND_ADDRESS =
  "https://polishelectionsimulation-dnevb2c4fse7dwc6.polandcentral-01.azurewebsites.net";

export function getBackendAddress(): string {
  const configuredAddress = import.meta.env.VITE_API_BASE_URL?.trim();
  if (configuredAddress) {
    return configuredAddress;
  }
  return DEFAULT_BACKEND_ADDRESS;
}

export function buildBackendUrl(path: string): string {
  const backendAddress = getBackendAddress().replace(/\/+$/, "");
  const normalizedPath = path.startsWith("/") ? path : `/${path}`;
  return `${backendAddress}${normalizedPath}`;
}
