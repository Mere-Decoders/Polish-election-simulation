export default class ResultsTableRow {
  public partyName: string
  public votes: number
  public percentVotes: number
  public seats: number
  public percentSeats: number
  
  public constructor(
      partyName: string, 
      votes: number, 
      percentVotes: number, 
      seats: number, 
      percentSeats: number ) {
    this.partyName = partyName
    this.votes = votes
    this.percentVotes = percentVotes
    this.seats = seats
    this.percentSeats = percentSeats
  }
}
