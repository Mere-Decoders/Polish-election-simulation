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
        :fill="resultsToDisplay[dot.partyIndex].color"
      />
    </svg>
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
  partyIndex: number
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
    
    dots.push({
      position: { orbit: currentOrbit, index: currentIndex },
      colorIndex: -1
    })

    currentOrbit++
    if (currentOrbit >= orbits.length) {
      currentIndex++
      currentOrbit = 0
      
      while (currentOrbit < orbits.length && orbits[currentOrbit]!.numberOfDots <= currentIndex) {
        currentOrbit++
      }
      
      if (currentOrbit >= orbits.length) {
        currentOrbit = 0
      }
    }
  }

  dots.sort((a, b) => -(calculateDotAngle(a) - calculateDotAngle(b)))

  for (let i = 0; i < desiredDots; i++) {
    if (partyIndex < resultsToDisplay.length) {
      if (seatIndex >= resultsToDisplay[partyIndex]!.seats) {
        seatIndex = 0
        partyIndex++
      }

      dots[i]!.partyIndex = partyIndex;
      seatIndex++
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
  const orbit = orbits.value[dot.position.orbit]!
  return props.outerRadius + orbit.radius * Math.cos(orbit.anglePerBall * (dot.position.index + 0.5))
}

const calculateDotY = (dot: Dot): number => {
  const orbit = orbits.value[dot.position.orbit]!
  return props.outerRadius - orbit.radius * Math.sin(orbit.anglePerBall * (dot.position.index + 0.5))
}

const calculateDotAngle = (dot: Dot): number => {
  const orbit = orbits.value[dot.position.orbit]!;
  return orbit.anglePerBall * dot.position.index;
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
}

svg {
  display: inline-block;
}

.legend-text {
  font-size: 0.875rem;
  text-align: justify;
  text-align-last: justify;
  text-justify: inter-word;
  font-weight: 500;
}

</style>
