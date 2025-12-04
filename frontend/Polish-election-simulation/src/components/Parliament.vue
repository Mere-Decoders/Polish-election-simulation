<template>
  <div class="parliament-container">
    <svg 
      :width="2 * outerRadius" 
      :height="outerRadius" 
      xmlns="http://www.w3.org/2000/svg"
    >
      <circle
        v-for="(dot, index) in dots"
        :key="index"
        :cx="calculateDotX(dot)"
        :cy="calculateDotY(dot)"
        :r="dotRadius * 0.7"
        :fill="getColorForDot(dot.colorIndex)"
      />
    </svg>
    
    <div class="legend-container">
      <div 
        v-for="(party, index) in resultsToDisplay"
        :key="index"
        class="legend-item"
      >
        <svg :width="dotRadius * 2" :height="dotRadius * 2">
          <circle 
            :cx="dotRadius" 
            :cy="dotRadius" 
            :r="dotRadius * 0.8" 
            :fill="getColorForDot(index)" 
          />
        </svg>
        <span class="legend-text"> &nbsp {{ party.seats }} ~ {{ party.partyName }} </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type ResultsTableRow from '../api/ResultsTableRow.ts'

interface Props {
  resultsToDisplay: ResultsTableRow[]
  innerRadius: number
  outerRadius: number
}

const props = defineProps<Props>()

const DESIRED_DOTS = 460

// Interfaces
interface Orbit {
  radius: number
  numberOfDots: number
  anglePerBall: number
}

interface ParliamentCoord {
  orbit: number
  index: number
}

interface Dot {
  position: ParliamentCoord
  colorIndex: number
}

// Utility functions
const circleArea = (radius: number): number => Math.PI * radius * radius

const calculateDotsFitting = (
  innerRadius: number,
  outerRadius: number,
  dotRadius: number
): number => {
  if (outerRadius <= innerRadius || Math.abs(outerRadius - innerRadius) < 2.0 * dotRadius) {
    throw new Error(
      `Invalid arguments: innerRadius(${innerRadius}), outerRadius(${outerRadius}), dotRadius(${dotRadius})`
    )
  }

  let result = 0
  for (let iRadius = innerRadius + dotRadius; iRadius <= outerRadius - dotRadius; iRadius += 2 * dotRadius) {
    const radialDotLength = 2 * Math.asin(dotRadius / iRadius)
    const numberOfDots = Math.floor(Math.PI / radialDotLength)
    result += numberOfDots
  }

  return result
}

const findOptimalDotRadius = (
  innerRadius: number,
  outerRadius: number,
  desiredDots: number
): number => {
  const totalArea = (circleArea(outerRadius) - circleArea(innerRadius)) / 2
  let lowerBound = Math.sqrt(totalArea / 2 / desiredDots / 4)
  let upperBound = Math.sqrt(totalArea * 3 / 2 / desiredDots / 4)

  let dotsUpper = calculateDotsFitting(innerRadius, outerRadius, lowerBound)
  let dotsLower = calculateDotsFitting(innerRadius, outerRadius, upperBound)

  if (!(dotsLower <= desiredDots && dotsUpper >= desiredDots)) {
    throw new Error(
      `Desired radius out of range [${dotsLower}, ${dotsUpper}] (${desiredDots} desired)`
    )
  }

  while (
    !(dotsLower === desiredDots || dotsUpper === desiredDots || Math.abs(lowerBound - upperBound) < 0.000001)
  ) {
    const nextBound = (lowerBound + upperBound) / 2.0
    const dotsNext = calculateDotsFitting(innerRadius, outerRadius, nextBound)

    if (dotsNext <= desiredDots) {
      upperBound = nextBound
      dotsLower = dotsNext
    } else {
      lowerBound = nextBound
      dotsUpper = dotsNext
    }
  }

  return dotsLower === desiredDots ? upperBound : lowerBound
}

const createOrbits = (innerRadius: number, outerRadius: number, dotRadius: number): Orbit[] => {
  const orbits: Orbit[] = []

  for (let iRadius = innerRadius + dotRadius; iRadius <= outerRadius - dotRadius; iRadius += 2 * dotRadius) {
    const radialDotLength = 2 * Math.asin(dotRadius / iRadius)
    const numberOfDots = Math.floor(Math.PI / radialDotLength)

    orbits.push({
      radius: iRadius,
      numberOfDots,
      anglePerBall: Math.PI / numberOfDots
    })
  }

  return orbits
}

const createDots = (orbits: Orbit[], resultsToDisplay: ResultsTableRow[], desiredDots: number): Dot[] => {
  const dots: Dot[] = []
  let currentOrbit = 0
  let currentIndex = 0
  let partyIndex = 0
  let seatIndex = 0

  for (let dotIndex = 0; dotIndex < desiredDots; dotIndex++) {
    let colorIndex = -1
    
    if (partyIndex < resultsToDisplay.length) {
      colorIndex = partyIndex
      seatIndex++
      if (seatIndex >= resultsToDisplay[partyIndex].seats) {
        seatIndex = 0
        partyIndex++
      }
    }

    dots.push({
      position: { orbit: currentOrbit, index: currentIndex },
      colorIndex
    })

    currentOrbit++
    if (currentOrbit >= orbits.length) {
      currentIndex++
      currentOrbit = 0
      
      while (currentOrbit < orbits.length && orbits[currentOrbit].numberOfDots <= currentIndex) {
        currentOrbit++
      }
      
      if (currentOrbit >= orbits.length) {
        currentOrbit = 0
      }
    }
  }

  return dots
}

// Computed properties
const dotRadius = computed(() => 
  findOptimalDotRadius(props.innerRadius, props.outerRadius, DESIRED_DOTS)
)

const orbits = computed(() => 
  createOrbits(props.innerRadius, props.outerRadius, dotRadius.value)
)

const dots = computed(() => 
  createDots(orbits.value, props.resultsToDisplay, DESIRED_DOTS)
)

// Methods to calculate positions
const calculateDotX = (dot: Dot): number => {
  const orbit = orbits.value[dot.position.orbit]
  return props.outerRadius + orbit.radius * Math.cos(orbit.anglePerBall * (dot.position.index + 0.5))
}

const calculateDotY = (dot: Dot): number => {
  const orbit = orbits.value[dot.position.orbit]
  return props.outerRadius - orbit.radius * Math.sin(orbit.anglePerBall * (dot.position.index + 0.5))
}

const getColorForDot = (colorIndex: number): string => {
  if (colorIndex === -1) {
    return 'hsl(0, 0%, 0%)'
  }

  const maxColorIndex = props.resultsToDisplay.length
  const hue = maxColorIndex > 0 ? (colorIndex / maxColorIndex) * 360 : 0
  return `hsl(${hue}, 100%, 50%)`
}
</script>

<style scoped>
.parliament-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.5rem;
  padding: 1.5rem;
}

.legend-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  columns: 2;
  margin-left: 3.8vw;

  gap: 1rem;
  justify-content: center;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.legend-text {
  font-size: 0.875rem;
  text-align: justify;
  text-align-last: justify;
  text-justify: inter-word;
  font-weight: 500;
}

</style>
