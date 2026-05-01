export default class VoteMapEditorColumn {
  public id: number
  public name: string
  public prev: string

  public constructor(
    id: number,
    name: string,
    prev: string
  ) {
    this.id = id
    this.name = name
    this.prev = prev
  }
}
