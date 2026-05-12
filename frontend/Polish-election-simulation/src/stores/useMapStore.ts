import { defineStore } from 'pinia';

export const useMapStore = defineStore("map", {
  state: () => ({
    selectedConstituency: 0 as string | number
  }),
  getters: {
    currentConstituency: state => state.selectedConstituency
  },
  actions: {
    selectConstituency(constituencyId: string | number) {
      this.selectedConstituency = this.selectedConstituency === constituencyId ? 0 : constituencyId;
    },
    clearConstituency() {
      this.selectedConstituency = 0;
    }
  }
})
