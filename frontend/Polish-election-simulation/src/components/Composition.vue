<template>
  <PageTabs :tabs="[
    { name: 'visualization', title: 'Wizualizacja wyników'},
    { name: 'methods', title: 'Edytor metod'},
    { name: 'constituencies', title: 'Edytor okręgów'}
  ]">
    <template #visualization>
      <VisualizationTab/>
    </template>

    <template #methods>
      <FillableSelect :elements="apiClient.getInstance().getApportionmentMethodsID()"/>
      <MethodsTab/>
    </template>

    <template #constituencies>
      <ConstituenciesTab/>
    </template>
  </PageTabs>
</template>

<script setup lang='ts'>
import PageTabs from './PageTabs.vue'
import FillableSelect from './FillableSelect.vue'

import VisualizationTab from './tabs/VisualizationTab.vue'
import MethodsTab from './tabs/MethodsTab.vue'
import ConstituenciesTab from './tabs/ConstituenciesTab.vue'

import apiClient from '../api/apiClient.ts'
import Results from '../api/Results.ts'

const api = apiClient.getInstance()
const methods = api.getApportionmentMethodsID()
</script>
