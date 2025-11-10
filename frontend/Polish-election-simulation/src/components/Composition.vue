<script setup lang='ts'>
  import PageTabs from './PageTabs.vue'
  import FillableSelect from './FillableSelect.vue';
  import apiClient from '../api/apiClient.ts';

  const api = apiClient.getInstance();
  const methods = api.getApportionmentMethodsID();
</script>

<template>
  <PageTabs :tabs="[
    { name: 'visualization', title: 'Kliknij by ujrzeć swoje jakże interesujące dane wyświetlone w atrakcyjnej postaci'},
    { name: 'methods', title: 'Dla nerdów'},
    { name: 'constituencies', title: 'Dla nołlajfów'}
  ]">
    <template #visualization>
    	Ale visualizacja, ale checa. Jakie ciekawe.
    </template>

    <template #methods>
    	Czyli jesteś nerdem?
      <FillableSelect :elements="apiClient.getInstance().getApportionmentMethodsID()"/>
    </template>

    <template #constituencies>
    	No edytuj sobie swoje okręgi, ja Ci nie bronię
    </template>
  </PageTabs>
</template>
