import ApportionmentMethodID from "./ApportionmentMethodID.ts"
import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";

export default class SimulationParametersNotice {
  public votesID: VotesID;
  public methodID: MethodID;
  public constituencySetID: ConstituencySetID;
  
  public constructor(votesID: VotesID, methodID: MethodID, constituencySetID: ConstituencySetID) {
    this.votesID = votesID;
    this.methodID = methodID;
    this.constituencySetID = constituencySetID;
  }
}
