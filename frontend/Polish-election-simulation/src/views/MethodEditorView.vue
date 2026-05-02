<template>
  <div class="toolbar">
    <div class="left">
      <InputSwitch v-model="vimMode" @change="applyMode" />
      <span>VIM mode</span>

      <Dropdown
        v-model="selectedMethod"
        :options="methods"
        placeholder="Select method"
        class="dropdown"
        @change="onMethodChange"
      />
    </div>

    <Button label="Save" icon="pi pi-save" @click="saveFile" />
  </div>

  <div ref="editor" id="editor"></div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from "vue";
import ace from "ace-builds";

// PrimeVue components (auto-registered or imported colally depending on the setup)
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";
import InputSwitch from "primevue/inputswitch";

// Ace editor setup
import "ace-builds/src-noconflict/mode-lua";
import "ace-builds/src-noconflict/theme-github";
import "ace-builds/src-noconflict/keybinding-vim";

const editor = ref(null);
let aceInstance = null;

const vimMode = ref(localStorage.getItem("vimMode") === "true");

// PrimeVue state
const selectedMethod = ref(null);
const methods = ref([
  // ENTRY POINT: cargar métodos dinámicamente aquí
  // { label: "Method 1", value: "method1" }
]);

onMounted(() => {
  aceInstance = ace.edit(editor.value);

  aceInstance.setTheme("ace/theme/github");
  aceInstance.session.setMode("ace/mode/lua");
  aceInstance.setShowPrintMargin(false);

  // ENTRY POINT:
  // fetch / load methods aquí si quieres
  aceInstance.setValue(`function hola()
  print("Hola mundo")
end`, -1);

  applyMode();

});

function applyMode() {
  localStorage.setItem("vimMode", vimMode.value);

  aceInstance.setKeyboardHandler(
    vimMode.value ? "ace/keyboard/vim" : null
  );
}

function onMethodChange() {
  // ENTRY POINT: lógica de selección de método
  console.log("Method:", selectedMethod.value);
}

function saveFile() {
  alert("File saved");
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

#editor {
  height: 300px;
  width: 100%;
  border-radius: 15px;
  border: 1px solid var(--color-border);
}
</style>
