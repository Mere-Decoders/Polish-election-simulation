<template>
  <svg :width="svgWidth" :height="svgHeight" v-if="constituencies && geoGenerator">
    <g class="map">
      <path
          v-for="(feature, index) in constituencies.features"
          :key="index"
          :d="geoGenerator(feature)"
          fill="#f00"
          stroke="#000"
          stroke-width="0.5"
      />
    </g>
  </svg>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { bbox } from "@turf/bbox";
import { geoMercator, geoPath, type GeoPath } from "d3-geo";
import { generateConstituencies } from "@/api/constituencyLoader.ts";

let constituencies = ref<any>(null);
const geoGenerator = ref<GeoPath>(geoPath().projection(geoMercator()));
const svgWidth = ref(800);
const svgHeight = ref(600);

onMounted(async () => {
  constituencies.value = await generateConstituencies();
  const bboxConstituencies = bbox(constituencies.value);
  const projection =
      geoMercator()
          .scale(2000) // TODO: Do dopracowania jaką wartość dokładnie dobrać
          .translate([0, 0])
          .center([bboxConstituencies[0], bboxConstituencies[3]]);

  geoGenerator.value = geoPath().projection(projection);
});
</script>