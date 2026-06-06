<template>
  <div class="toolbar">
    <div class="left">
      <InputSwitch v-model="vimMode" @change="applyMode" />
      <span>VIM mode</span>

      <Dropdown
        v-model="selectedMethod"
        :options="methods"
        optionLabel="name"
        optionValue="id"
        placeholder="Select method"
        class="dropdown"
        @change="onMethodChange"
      />
    </div>

    <div class="right">
      <Button label="New" icon="pi pi-plus" severity="secondary" @click="showNewMethodDialog = true" />
      <Button label="Save" icon="pi pi-save" :loading="saving" :disabled="!selectedMethod" @click="saveFile" />
    </div>
  </div>

  <div ref="editor" id="editor"></div>

  <Dialog v-model:visible="showNewMethodDialog" header="New Method" modal :style="{ width: '25rem' }">
    <div class="dialog-body">
      <label for="method-name">Method name</label>
      <InputText id="method-name" v-model="newMethodName" placeholder="Enter method name" autofocus />
    </div>
    <template #footer>
      <Button label="Cancel" severity="secondary" @click="showNewMethodDialog = false" />
      <Button label="Create" icon="pi pi-check" :loading="creating" @click="createMethod" />
    </template>
  </Dialog>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from "vue";
import ace from "ace-builds";

import Button from "primevue/button";
import Dialog from "primevue/dialog";
import Dropdown from "primevue/dropdown";
import InputSwitch from "primevue/inputswitch";
import InputText from "primevue/inputtext";

import "ace-builds/src-noconflict/mode-lua";
import "ace-builds/src-noconflict/theme-github";
import "ace-builds/src-noconflict/keybinding-vim";

import apiClient from "@/api/apiClient.ts";

const editor = ref(null);
let aceInstance = null;

const vimMode = ref(localStorage.getItem("vimMode") === "true");

const selectedMethod = ref(null);
const currentMethodName = ref("");
const methods = ref([]);

const showNewMethodDialog = ref(false);
const newMethodName = ref("");
const creating = ref(false);
const saving = ref(false);

onMounted(async () => {
  aceInstance = ace.edit(editor.value);
  aceInstance.setTheme("ace/theme/github");
  aceInstance.session.setMode("ace/mode/lua");
  aceInstance.setShowPrintMargin(false);
  aceInstance.setValue("", -1);
  applyMode();

  methods.value = await apiClient.getApportionmentMethodIDs();
});

function applyMode() {
  localStorage.setItem("vimMode", vimMode.value);
  aceInstance.setKeyboardHandler(vimMode.value ? "ace/keyboard/vim" : null);
}

async function onMethodChange() {
  const details = await apiClient.getApportionmentMethodDetails(selectedMethod.value);
  currentMethodName.value = details.name;
  aceInstance.setValue(details.luaCode, -1);
}

async function saveFile() {
  saving.value = true;
  try {
    await apiClient.updateApportionmentMethod({
      id: selectedMethod.value,
      name: currentMethodName.value,
      luaCode: aceInstance.getValue(),
    });
  } finally {
    saving.value = false;
  }
}

async function createMethod() {
  const label = newMethodName.value.trim();
  if (!label) return;

  creating.value = true;
  try {
    const created = await apiClient.createApportionmentMethod(label, aceInstance.getValue());
    methods.value = await apiClient.getApportionmentMethodIDs();
    selectedMethod.value = created.id;
    currentMethodName.value = created.name;
    showNewMethodDialog.value = false;
    newMethodName.value = "";
  } finally {
    creating.value = false;
  }
}

onBeforeUnmount(() => {
  if (aceInstance) {
    aceInstance.destroy();
    aceInstance = null;
  }
});
</script>

<style>
.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px;
  margin-bottom: 10px;
}

.left {
  display: flex;
  align-items: center;
  gap: 10px;
}

.right {
  display: flex;
  align-items: center;
  gap: 10px;
}

.dialog-body {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding-bottom: 1rem;
}

#editor {
  height: 300px;
  width: 100%;
  border-radius: 15px;
  border: 1px solid var(--color-border);
}
</style>
