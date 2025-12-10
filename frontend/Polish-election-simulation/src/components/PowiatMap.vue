<template>
  <svg ref="svgRef" class="constituencies-svg" v-if="constituencies && geoGenerator">
    <g class="map">
      <path
          class="constituency"
          v-for="(feature, index) in constituencies.features"
          :key="index"
          :d="geoGenerator(feature)!"
          stroke="#000"
          stroke-width="0.5"
      />
    </g>
  </svg>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { geoMercator, geoPath, type GeoPath } from "d3-geo";
import { generateConstituencies } from "@/api/constituencyLoader.ts";

const geoGenerator = ref<GeoPath>(geoPath().projection(geoMercator()));
const svgRef = ref<SVGSVGElement | null>(null);

const props = defineProps<{
  constituencies: any
}>();

onMounted(async () => {
  await new Promise(resolve => setTimeout(resolve, 0));
  if (svgRef.value) {
    const width = svgRef.value.clientWidth;
    const height = svgRef.value.clientHeight;

    const projection = geoMercator();
    // Create projection fitted to actual size
    projection.fitSize([width, height], props.constituencies);

    geoGenerator.value = geoPath().projection(projection);
  }
});
</script>

<style scoped>

.constituencies-svg {
  width: 100%;
  height: 100%;
}

.constituency {
  fill: var(--color-constituency);
}

</style>