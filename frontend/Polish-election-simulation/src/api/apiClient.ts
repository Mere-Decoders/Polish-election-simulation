import ApportionmentMethodID from "./ApportionmentMethodID.ts"
import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";

export default class apiClient {
  private static instance: apiClient;
    // Private constructor prevents direct instantiation
  private constructor() {}
    // Static method to access the single instance
  public static getInstance(): apiClient {
    if (!apiClient.instance) {
      apiClient.instance = new apiClient();
    }
    return apiClient.instance;
  }

  public getMethods(): ApportionmentMethodID[] {
    let result: ApportionmentMethodID[] = [
        new ApportionmentMethodID("D'Hondta"),
        new ApportionmentMethodID("Sainte-Lague")
    ];
    return result;
  }

  public getVotes(): VotesID[] {
    let result: VotesID[] = [
        new VotesID("2019"),
        new VotesID("2023")
    ];
    return result;
  }

  public getConstituencySets(): ConstituencySetID[] {
    let result: ConstituencySetID[] = [
        new ConstituencySetID("Oficjalne"),
        new ConstituencySetID("Zestaw Lorem ipsum")
    ];
    return result;
  }
}
