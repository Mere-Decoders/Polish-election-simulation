import ApportionmentMethodID from "./ApportionmentMethodID.ts"
import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";
import ResultsTableRow from "@/api/ResultsTableRow.ts";

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

  public async getTotalResults(year: number, method: string): Promise<ResultsTableRow[]>  {
    const data_response = await fetch("/mock_results.json");
    const data = (await data_response.json()).result;
    let results: ResultsTableRow[] = [];
    const constituencyCount: number = 41;
    let sumSeatsArray: Array<number> = [];
    let sumVotesArray: Array<number> = [];
    for (let i = 0; i < data.partyNames.length; i++) {
        let sumVotes: number = 0;
        let sumSeats: number = 0;
        for (let j = 1; j <= constituencyCount; j++) {
            sumVotes += data.constituencyVotes[j][i];
            sumSeats += data.constituencySeats[j][i];
        }
        sumSeatsArray.push(sumSeats);
        sumVotesArray.push(sumVotes);
    }
    const totalSumSeats: number = sumSeatsArray.reduce((a, b) => a + b);
    const totalSumVotes: number = sumVotesArray.reduce((a, b) => a + b);

    for (let i = 0; i < data.partyNames.length; i++) {
        results.push(
            new ResultsTableRow(
                data.partyNames[i],
                sumVotesArray[i]!,
                sumVotesArray[i]! / totalSumVotes,
                sumSeatsArray[i]!,
                sumSeatsArray[i]! / totalSumSeats));
    }
    return results;
  }
}
