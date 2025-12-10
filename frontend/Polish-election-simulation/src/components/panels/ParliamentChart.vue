<script setup lang="ts">
import { watch } from "vue";
const props = defineProps({
  data: {
    type: Array,
    required: true,
  }
});

import * as d3 from 'd3';
import * as d3ParliamentChart from 'd3-parliament-chart';
import { ref, onMounted } from 'vue';

const parliamentChartSVG = ref(null);

onMounted(() => {
  let pcsvg = d3.select(parliamentChartSVG.value);
  console.log(pcsvg);
  pcsvg.call(
    d3ParliamentChart.parliamentChart()
      .aggregatedData([
        {"seats":40,"color":"#636cbb"},
        {"seats":30,"color":"#26d050"},
        {"seats":50,"color":"#315cee"}
      ])
      .sections(1)
      .seatRadius(5)
      .rowHeight(20)
      .debug(true),
  );
});
</script>

<template>
  <svg ref="parliamentChartSVG" width="100%" height="100%"/>
</template>

<style scoped>

</style>