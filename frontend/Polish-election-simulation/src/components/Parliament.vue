<template>
  <div>
    <svg :width="2 * outerRadius" :height="outerRadius" xmlns="http://www.w3.org/2000/svg">
      <circle
        v-for="(dot, index) in dots"
        :key="index"
        :cx="outerRadius + orbits[dot.position.orbit].radius * Math.cos(orbits[dot.position.orbit].anglePerBall * (dot.position.index + 0.5))"
        :cy="outerRadius - orbits[dot.position.orbit].radius * Math.sin(orbits[dot.position.orbit].anglePerBall * (dot.position.index + 0.5))"
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
          <circle :cx="dotRadius" :cy="dotRadius" :r="dotRadius * 0.8" :fill="getColorForDot(index)" />
        </svg>
        <span class="legend-text">{{ party.partyName }} ({{ party.seats }})</span>
      </div>
    </div> 
  </div>
</template>

<script setup lang='ts'>
  import ResultsTableRow from '../api/ResultsTableRow.ts'

  const props = defineProps<{
    resultsToDisplay: ResultsTableRow[]
    innerRadius: number
    outerRadius: number
  }>()

  var innerRadius: number = props.innerRadius
  var outerRadius: number = props.outerRadius
  
  var dotRadius: number
  const desiredDots: number = 460

  {
    function circleArea(R: number): number {
      return Math.PI * R * R
    }

    function dotsFitting(innerRadius: number, outerRadius: number, dotRadius: number) {
      if (outerRadius <= innerRadius || Math.abs(outerRadius - innerRadius) < 2.0 * dotRadius)
        throw new Error(`Improper arguments: innerRadius(${innerRadius}), outerRadius(${outerRadius}) and dotRadius(${dotRadius})`)

      let result: number = 0
      for (let iRadius = innerRadius + dotRadius; iRadius <= outerRadius - dotRadius; iRadius += 2 * dotRadius) {
        var radialDotLength = 2 * Math.asin(dotRadius / iRadius)
        var numberOfDots = Math.floor(Math.PI / radialDotLength)
        result += numberOfDots
      }

      return result
    }

    var totalArea: number = (circleArea(outerRadius) - circleArea(innerRadius)) / 2
    var lowerBound: number = Math.sqrt(totalArea / 2 / desiredDots / 4) // Approximate number of circles that fit in the given area
    var upperBound: number = Math.sqrt(totalArea * 3 / 2 / desiredDots / 4) // (1/2 of the total area and 3/2 of the total area)

    var dotsUpper = dotsFitting(innerRadius, outerRadius, lowerBound)
    var dotsLower = dotsFitting(innerRadius, outerRadius, upperBound)

    if (!(dotsLower <= desiredDots && dotsUpper >= desiredDots))
      throw new Error(`The desired radius outside of range [${dotsLower}, ${dotsUpper}] (${desiredDots} desired)`)

    console.log(`dotsLower = ${dotsLower}, dotsUpper = ${dotsUpper}`)
    
    while (!(dotsLower === desiredDots || dotsUpper === desiredDots || Math.abs(lowerBound- upperBound) < 0.000001)) {
      var nextBound = (lowerBound + upperBound) / 2.0
      var dotsNext = dotsFitting(innerRadius, outerRadius, nextBound)

      if (dotsNext <= desiredDots) {
        upperBound = nextBound
	dotsLower = dotsNext
      }
      else {
        lowerBound = nextBound
	dotsUpper = dotsNext
      }

      console.log(`dotsLower = ${dotsLower}, dotsUpper = ${dotsUpper}, upperBound = ${upperBound}, lowerBound = ${lowerBound}`)
    }

    if (dotsLower === desiredDots)
      dotRadius = upperBound
    else if (dotsUpper  === desiredDots)
      dotRadius = lowerBound
    else
      dotRadius = lowerBound
  }

  class Orbit {
    radius: Number = 0
    numberOfDots: Number = 0
    anglePerBall: Number = 0
  }

  var orbits: Orbit[] = []
  var total_dots = 0

  {
    for(let iRadius = innerRadius + dotRadius; iRadius <= outerRadius - dotRadius; iRadius += 2 * dotRadius) {
      var nextOrbit: Orbit = new Orbit()
      nextOrbit.radius = iRadius

      var radialDotLength = 2 * Math.asin(dotRadius / iRadius)
      nextOrbit.numberOfDots = Math.floor(Math.PI / radialDotLength)
      nextOrbit.anglePerBall = Math.PI / nextOrbit.numberOfDots
      orbits.push(nextOrbit)

      total_dots += nextOrbit.numberOfDots
    }
  }

  class ParliamentCoord {
    orbit: Number = 0
    index: Number = 0

    next(): ParliamentCoord {
      var lastCoord: ParliamentCoord = structuredClone(this)

      this.orbit++
      if (this.orbit >= orbits.length) {
        this.index++
        this.orbit = 0
	for (const orbitIterator of orbits)
          if (orbitIterator.numberOfDots <= this.index) {
            this.orbit++
	  }
      }

      return lastCoord
    }
  }

  console.log(props.resultsToDisplay)

  class SeatIterator {
    party: Number = 0
    index: Number = 0

    next() {
      if (this.party >= props.resultsToDisplay.length)
        return -1

      var lastParty = this.party
      this.index++
      if (this.index >= props.resultsToDisplay[this.party].seats) {
        this.index = 0
	this.party++
      }
      return lastParty
    }
  }

  class Dot {
    position: ParliamentCoord 
    colorIndex: Number 
  }

  var dots: Dot[] = []

  var beginning = new ParliamentCoord()
  var seatIterator = new SeatIterator()
  for (var dotIndex = 0; dotIndex < desiredDots; dotIndex++) {
    var dot = new Dot()
    dot.position = beginning.next()
    dot.colorIndex = seatIterator.next()
    dots.push(dot)
  }

  console.log(`total dots = ${total_dots}`)
  console.log(orbits)

  function getColorForDot(colorIndex: number): string {
    if (colorIndex === -1) {
      return 'hsl(0, 0%, 0%)'
    }
  
    const maxColorIndex = props.resultsToDisplay.length
    const hue = maxColorIndex > 0 ? (colorIndex / maxColorIndex) * 360 : 0
    return `hsl(${hue}, 100%, 50%)`
  }

  console.log(dots)
</script>
