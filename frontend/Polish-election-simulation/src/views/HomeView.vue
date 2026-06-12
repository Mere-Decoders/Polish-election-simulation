<template>
  <div class="bg-white">
    <div class="mx-auto max-w-6xl px-6 pt-5 pb-16">
      <div class="text-center">
        <h1 class="mb-6 text-5xl font-bold tracking-tight text-gray-900">
          Polish Election Simulation
        </h1>

        <p class="mx-auto py-10 text-xl text-gray-600">
          Explore electoral systems, compare seat allocation methods,
          and simulate election outcomes using official voting data.
        </p>
      </div>

      <div class="mt-16 grid gap-8 md:grid-cols-3">
        <div class="rounded-2xl border border-gray-200 p-8 shadow-sm">
          <h2 class="mb-3 text-xl font-semibold">
            Official Election Data
          </h2>

          <p class="text-gray-600">
            Analyze real election results and discover how different
            electoral systems affect the distribution of seats.
          </p>
        </div>

        <div class="rounded-2xl border border-gray-200 p-8 shadow-sm">
          <h2 class="mb-3 text-xl font-semibold">
            Custom Simulations
          </h2>

          <p class="text-gray-600">
            Modify vote counts, create your own datasets, and redesign
            electoral districts to test alternative scenarios.
          </p>
        </div>

        <div class="rounded-2xl border border-gray-200 p-8 shadow-sm">
          <h2 class="mb-3 text-xl font-semibold">
            Lua Algorithm Editor
          </h2>

          <p class="text-gray-600">
            Develop and execute custom seat allocation algorithms
            directly in the browser using Lua scripting.
          </p>
        </div>
      </div>

      <div class="h-10"></div>

      <div class="mt-16 rounded-3xl bg-gray-50 p-10 text-center">
        <h2 class="mb-4 text-3xl font-semibold text-gray-900">
          Learn How Electoral Systems Shape Results
        </h2>

        <p class="mx-auto text-gray-600 pt-5">
          Compare methods such as D'Hondt, Sainte-Lague,
          and your own custom implementations. Observe how small changes
          in votes or district boundaries can dramatically influence the
          final composition of parliament.
        </p>
      </div>
    </div>
  </div>
</template>