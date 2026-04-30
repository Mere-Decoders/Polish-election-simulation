export default class DetailedResultsRow {
  public partyName: string
  public votes: number[]
  public seats: number[]

  public constructor(
    partyName: string,
    votes: number[],
    seats: number[]
  ) {
    this.partyName = partyName;
    this.votes = votes;
    this.seats = seats;
  }
}