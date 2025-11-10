import PartyConstituencyResults from './PartyConstituencyResults.ts'

export default class Results {
  public constituencies: Record<number, PartyConstituencyResults[]>;

  public constructor() {
    this.constituencies = {}
  }
}
