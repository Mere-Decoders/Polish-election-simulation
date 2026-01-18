export default class ResultsTableRow {
  public partyName: string
  public color: string
  public votes: number
  public percentVotes: number
  public seats: number
  public percentSeats: number
  
  public constructor(
      partyName: string, 
      color: string,
      votes: number, 
      percentVotes: number, 
      seats: number, 
      percentSeats: number ) {
    this.partyName = partyName
    this.color = color
    this.votes = votes
    this.percentVotes = percentVotes
    this.seats = seats
    this.percentSeats = percentSeats
  }
}
