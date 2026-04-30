import { defineStore } from 'pinia';

export const useMapStore = defineStore("map", {
  state: () => ({
    selectedConstituency: 0
  }),
  getters: {
    currentConstituency: state => state.selectedConstituency
  },
  actions: {
    selectConstituency(constituencyId: number) {
      this.selectedConstituency = constituencyId;
    }
  }
})