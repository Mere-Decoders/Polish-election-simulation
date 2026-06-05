export interface Party {
  name: string;
  isCoalition: boolean;
  needsThreshold: boolean;
}

export interface  District {
  seats: number;
  terytCodes: string[];
}

export interface SimulationData {
  parties: Party[];
  districts: {
    [key: string]: District;
  };
  votesInArea: {
    [key: string]: number[];
  };
}