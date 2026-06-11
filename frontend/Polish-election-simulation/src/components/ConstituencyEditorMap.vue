<template>
  <svg ref="svgRef" class="constituencies-svg" v-if="constituencies && geoGenerator">
    <g class="map">
      <path
          class="constituency"
          v-for="(feature, index) in constituencies.features"
          :key="index"
          :d="geoGenerator(feature)!"
          :fill="fillMap?.[feature.properties.terc] ?? 'var(--color-constituency)'"
          stroke="#000"
          :stroke-width="highlighted.has(feature.properties.terc) ? 1.25 : 0.5"
          @click="$emit('feature-click', feature.properties.terc)"
      />
    </g>
  </svg>
</template>

<script setup lang="ts">
import {ref, onMounted, onUnmounted, computed} from 'vue';
import { geoMercator, geoPath, type GeoPath } from "d3-geo";
import { generateConstituencies } from "@/api/constituencyLoader.ts";

const geoGenerator = ref<GeoPath>(geoPath().projection(geoMercator()));
const svgRef = ref<SVGSVGElement | null>(null);
let resizeObserver: ResizeObserver | null = null;

defineEmits<{ 'feature-click': [teryt: string] }>()
const props = defineProps<{
  constituencies: any
  fillMap?: Record<string, string>
  highlightedPowiats: Set<string> | undefined
}>()

const highlighted = computed(() =>
    props.highlightedPowiats ?? new Set<string>()
);

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

.constituencies-svg {
  width: 100%;
  height: 100%;
  display: block;
}

.constituency {
  cursor: pointer;
  transition: filter 0.1s ease;
}

.constituency:hover {
  stroke: #f00;
  stroke-width: 1.5;
}

</style>