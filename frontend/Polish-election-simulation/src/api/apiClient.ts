import ApportionmentMethodID from "./ApportionmentMethodID.ts"
import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";
import type ResultsTableRow from "@/api/ResultsTableRow.ts";

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

  public getApportionmentMethodsID(): ApportionmentMethodID[] {
    let result: ApportionmentMethodID[] = [
        new ApportionmentMethodID("D'Hondta"),
        new ApportionmentMethodID("Sainte-Lague")
    ];
    return result;
  }

  public getVotesID(): VotesID[] {
    let result: VotesID[] = [
        new VotesID("2019"),
        new VotesID("2023")
    ];
    return result;
  }

  public getConstituencySetsID(): ConstituencySetID[] {
    let result: ConstituencySetID[] = [
        new ConstituencySetID("Oficjalne"),
        new ConstituencySetID("Zestaw Lorem ipsum")
    ];
    return result;
  }

  public async getTotalResults(year: number, method: string)  {
    const data = await fetch("/mock_results.json");

  }
}
