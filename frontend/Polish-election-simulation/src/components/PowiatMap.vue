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
import { ref, onMounted, onUnmounted } from 'vue';
import { geoMercator, geoPath, type GeoPath } from "d3-geo";
import { generateConstituencies } from "@/api/constituencyLoader.ts";

const geoGenerator = ref<GeoPath>(geoPath().projection(geoMercator()));
const svgRef = ref<SVGSVGElement | null>(null);
let resizeObserver: ResizeObserver | null = null;

const props = defineProps<{
  constituencies: any
}>();

const updateProjection = () => {
  if (svgRef.value) {
    const width = svgRef.value.clientWidth;
    const height = svgRef.value.clientHeight;

    if (width > 0 && height > 0) {
      const projection = geoMercator();
      // Create projection fitted to actual size
      projection.fitSize([width, height], props.constituencies);
      geoGenerator.value = geoPath().projection(projection);
    }
  }
};

onMounted(async () => {
  await new Promise(resolve => setTimeout(resolve, 0));
  updateProjection();

  // Watch for container resize and recalculate projection
  if (svgRef.value) {
    resizeObserver = new ResizeObserver(() => {
      updateProjection();
    });
    resizeObserver.observe(svgRef.value);
  }
});

onUnmounted(() => {
  if (resizeObserver) {
    resizeObserver.disconnect();
  }
});
</script>

<style scoped>
  @reference "tailwindcss";

  .constituencies-svg {
    @apply w-full h-full block;
  }
  .constituency {
    @apply fill-[var(--color-constituency)];
  }


</style>