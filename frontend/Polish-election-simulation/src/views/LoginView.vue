<template>
  <section class="auth-view">
    <Card class="auth-card">
      <template #title>
        {{ isRegisterMode ? "Create account" : "Login" }}
      </template>
      <template #content>
        <div class="mode-switch">
          <Button
            label="Login"
            :outlined="isRegisterMode"
            :disabled="isSubmitting"
            @click="switchMode(false)"
          />
          <Button
            label="Register"
            :outlined="!isRegisterMode"
            :disabled="isSubmitting"
            @click="switchMode(true)"
          />
        </div>

        <form class="auth-form" @submit.prevent="submit">
          <div v-if="isRegisterMode" class="field-group">
            <label for="email">Email</label>
            <InputText
              id="email"
              v-model.trim="form.email"
              type="email"
              autocomplete="email"
              required
              maxlength="100"
            />
          </div>

          <div class="field-group">
            <label for="username">Username</label>
            <InputText
              id="username"
              v-model.trim="form.username"
              type="text"
              autocomplete="username"
              required
              minlength="3"
              maxlength="32"
            />
          </div>

          <div class="field-group">
            <label for="password">Password</label>
            <Password
              id="password"
              v-model="form.password"
              :feedback="isRegisterMode"
              toggleMask
              autocomplete="current-password"
              required
              minlength="8"
              fluid
            />
          </div>

          <div v-if="isRegisterMode" class="field-group">
            <label for="confirmPassword">Confirm password</label>
            <Password
              id="confirmPassword"
              v-model="form.confirmPassword"
              :feedback="false"
              toggleMask
              autocomplete="new-password"
              required
              minlength="8"
              fluid
            />
          </div>

          <Message v-if="errorMessage" severity="error" :closable="false">
            {{ errorMessage }}
          </Message>

          <Button
            type="submit"
            class="submit-button"
            :label="submitButtonLabel"
            :loading="isSubmitting"
            :disabled="isSubmitting"
          />
        </form>
      </template>
    </Card>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import Card from "primevue/card";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Button from "primevue/button";
import Message from "primevue/message";
import { useAuth } from "@/auth/useAuth";

const route = useRoute();
const router = useRouter();
const auth = useAuth();

const isRegisterMode = ref(false);
const isSubmitting = ref(false);
const errorMessage = ref("");

const form = reactive({
  email: "",
  username: "",
  password: "",
  confirmPassword: "",
});

const redirectPath = computed(() => {
  const redirect = route.query.redirect;
  if (typeof redirect === "string" && redirect.startsWith("/")) {
    return redirect;
  }
  return "/simulation";
});

const submitButtonLabel = computed(() => {
  if (isSubmitting.value) {
    return isRegisterMode.value ? "Creating account..." : "Logging in...";
  }
  return isRegisterMode.value ? "Create account" : "Login";
});

onMounted(async () => {
  const hasSession = await auth.ensureSession();
  if (hasSession) {
    await router.replace(redirectPath.value);
  }
});

function switchMode(registerMode: boolean) {
  isRegisterMode.value = registerMode;
  errorMessage.value = "";
}

async function submit() {
  isSubmitting.value = true;
  errorMessage.value = "";

  try {
    if (isRegisterMode.value) {
      if (form.password !== form.confirmPassword) {
        throw new Error("Passwords do not match.");
      }

      await auth.register(form.email, form.username, form.password);
    } else {
      await auth.login(form.username, form.password);
    }

    await router.replace(redirectPath.value);
  } catch (error) {
    if (error instanceof Error) {
      errorMessage.value = error.message;
    } else {
      errorMessage.value = "Authentication failed.";
    }
  } finally {
    isSubmitting.value = false;
  }
}
</script>

<style scoped>
.auth-view {
  display: flex;
  justify-content: center;
  margin: 2rem 0;
}

.auth-card {
  width: min(520px, 100%);
}

.mode-switch {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.auth-form {
  display: flex;
  flex-direction: column;
  gap: 0.9rem;
}

.field-group {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.submit-button {
  margin-top: 0.4rem;
}
</style>
