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

  <div class="api-docs">
    <h3>Scripting API</h3>
    <p>
      The script is invoked once per electoral district. It must return a Lua table <code>t</code>
      indexed <code>1..TotalParties()</code> where <code>t[i]</code> is the number of seats awarded
      to party <code>i</code> in the district. The values must sum to <code>DistrictSeats()</code>.
    </p>

    <h4>Available functions</h4>
    <table class="api-table">
      <thead>
        <tr><th>Function</th><th>Returns</th><th>Description</th></tr>
      </thead>
      <tbody>
        <tr>
          <td><code>TotalParties()</code></td>
          <td>number</td>
          <td>Total number of parties in the election.</td>
        </tr>
        <tr>
          <td><code>DistrictSeats()</code></td>
          <td>number</td>
          <td>Number of seats to be filled in the current district.</td>
        </tr>
        <tr>
          <td><code>DistrictPartyVotes(i)</code></td>
          <td>number</td>
          <td>Votes cast for party <code>i</code> in the current district.</td>
        </tr>
        <tr>
          <td><code>NationalPartyVotes(i)</code></td>
          <td>number</td>
          <td>Total votes cast for party <code>i</code> across all districts (used for threshold checks).</td>
        </tr>
        <tr>
          <td><code>NationalTotalVotes()</code></td>
          <td>number</td>
          <td>Total votes cast across all parties and all districts.</td>
        </tr>
        <tr>
          <td><code>NeedsThreshold(i)</code></td>
          <td>boolean</td>
          <td>
            Returns <code>true</code> if party <code>i</code> must clear an electoral threshold to
            receive seats. Some parties (e.g. ethnic minority lists) are exempt and always return
            <code>false</code>.
          </td>
        </tr>
        <tr>
          <td><code>IsCoalition(i)</code></td>
          <td>boolean</td>
          <td>
            Returns <code>true</code> if party <code>i</code> is an electoral coalition. Coalitions
            are typically held to a higher threshold than single parties.
          </td>
        </tr>
      </tbody>
    </table>

    <h4>Conventions</h4>
    <ul>
      <li>Party indices are 1-based (<code>1..TotalParties()</code>).</li>
      <li>
        Threshold checks should use <code>NationalPartyVotes(i) / NationalTotalVotes()</code> and
        respect both <code>NeedsThreshold(i)</code> and <code>IsCoalition(i)</code>.
      </li>
      <li>
        Parties that do not pass the threshold should receive 0 seats and must still appear in the
        returned table.
      </li>
      <li>
        The script has no access to results from other districts; each district is computed
        independently.
      </li>
    </ul>
  </div>

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

.api-docs {
  margin-top: 2rem;
  padding: 1.25rem 1.5rem;
  border: 1px solid var(--color-border);
  border-radius: 12px;
  font-size: 0.9rem;
  line-height: 1.6;
}

.api-docs h3 {
  margin: 0 0 0.5rem;
}

.api-docs h4 {
  margin: 1.25rem 0 0.5rem;
}

.api-docs p,
.api-docs ul {
  margin: 0.25rem 0;
}

.api-docs ul {
  padding-left: 1.5rem;
}

.api-docs code {
  padding: 0.1em 0.35em;
  border-radius: 4px;
  font-size: 0.85em;
  background: var(--color-background-mute, #f3f3f3);
}

.api-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 0.25rem;
}

.api-table th,
.api-table td {
  text-align: left;
  padding: 0.4rem 0.75rem;
  border-bottom: 1px solid var(--color-border);
}

.api-table th {
  font-weight: 600;
}
</style>
