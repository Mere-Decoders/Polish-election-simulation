import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";
import ResultsTableRow from "@/api/ResultsTableRow.ts";
import get_color_for_index from "@/api/get_color_for_index.ts";
import type ApportionmentMethod from "@/api/ApportionmentMethod.ts";
import { buildBackendUrl } from "@/api/buildBackendUrl.ts";
import { authFetch } from "@/auth/useAuth.ts";

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

  private static async ensureSuccess(response: Response): Promise<void> {
    if (response.ok) {
      return;
    }

    const errorText = await response.text().catch(() => "");
    if (errorText.length > 0) {
      throw new Error(errorText);
    }

    throw new Error(`Request failed with status ${response.status}`);
  }

  private static async authenticatedGet(path: string): Promise<Response> {
    const response = await authFetch(buildBackendUrl(path), {
      headers: {
        accept: "application/json",
      },
    });

    await apiClient.ensureSuccess(response);
    return response;
  }

  // All methods can be static and if needed access the data in the singleton by using getInstance()
  // Making them static makes the invocation cleaner (you don't need to use the getInstance() method)
  public static async getApportionmentMethodIDs(): Promise<ApportionmentMethod[]> {
    const data_response = await apiClient.authenticatedGet("/api/methods/Method/get-list");
    return await data_response.json();
  }

  public static async getVotesIDs(): Promise<VotesID[]> {
    const data_response = await apiClient.authenticatedGet("/api/sim-data/SimulationData/get-list");
    return await data_response.json();
  }

  public static getConstituencySetIDs(): ConstituencySetID[] {
    let result: ConstituencySetID[] = [
      new ConstituencySetID("Oficjalne"),
      new ConstituencySetID("Zestaw Lorem ipsum")
    ];
    return result;
  }

  public static async getTotalResults(sim_data: string, method: string): Promise<ResultsTableRow[]>  {
    const mockup = false;
    let data_response;
    if (mockup) {
      data_response = await apiClient.authenticatedGet("/mock_results.json");
    }
    else {
      data_response = await apiClient.authenticatedGet(
        "/api/Simulation?" + new URLSearchParams({ simDataGuid: sim_data, methodGuid: method})
      );
    }
    const data = await data_response.json();
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
                get_color_for_index(i, data.partyNames.length),
                sumVotesArray[i]!,
                sumVotesArray[i]! / totalSumVotes,
                sumSeatsArray[i]!,
                sumSeatsArray[i]! / totalSumSeats));
    }
    return results;
  }
}
