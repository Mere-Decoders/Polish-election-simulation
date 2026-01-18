import ConstituencySetID from "./ConstituencySetID.ts";
import VotesID from "./VotesID.ts";
import ResultsTableRow from "@/api/ResultsTableRow.ts";
import get_color_for_index from "@/api/get_color_for_index.ts";
import type ApportionmentMethod from "@/api/ApportionmentMethod.ts";

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

  private static getBackendAddress() {
    return "https://polishelectionsimulation-dnevb2c4fse7dwc6.polandcentral-01.azurewebsites.net"
  }

  // All methods can be static and if needed access the data in the singleton by using getInstance()
  // Making them static makes the invocation cleaner (you don't need to use the getInstance() method)
  public static async getApportionmentMethodIDs(): Promise<ApportionmentMethod[]> {
    const backend_address = apiClient.getBackendAddress();
    const auth_token = await apiClient.getAuthToken("kamil", "kamilslimak");
    const data_response = await fetch(
      backend_address + "/api/methods/Method/get-list",
      {
        headers: {
          "accept": "text/plain",
          "Authorization": "Bearer " + auth_token
        }
      }
    );
    return await data_response.json();
  }

  public static async getVotesIDs(): Promise<VotesID[]> {
    const backend_address = apiClient.getBackendAddress();
    const auth_token = await apiClient.getAuthToken("kamil", "kamilslimak");
    const data_response = await fetch(
      backend_address + "/api/sim-data/SimulationData/get-list",
      {
        headers: {
          "accept": "text/plain",
          "Authorization": "Bearer " + auth_token
        }
      }
    );
    return await data_response.json();
  }

  public static getConstituencySetIDs(): ConstituencySetID[] {
    let result: ConstituencySetID[] = [
      new ConstituencySetID("Oficjalne"),
      new ConstituencySetID("Zestaw Lorem ipsum")
    ];
    return result;
  }

  public static async getAuthToken(username: string, password: string): Promise<string> {
    const backend_address = apiClient.getBackendAddress();
    const auth = await fetch(
      backend_address + "/api/Auth/login",
      {
        method: "POST",
        headers: {
          "accept": "*/*",
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ username: username, password: password })
      }
    );
    return (await auth.json()).token;
  }

  public static async getTotalResults(sim_data: string, method: string): Promise<ResultsTableRow[]>  {
    const mockup = false;
    let data_response;
    if (mockup) {
      data_response = await fetch("/mock_results.json");
    }
    else {
      const backend_address = apiClient.getBackendAddress();
      const auth_token = await apiClient.getAuthToken("kamil", "kamilslimak");
      data_response = await fetch(
        backend_address + "/api/Simulation?" + new URLSearchParams({ simDataGuid: sim_data, methodGuid: method}),
        {
          headers: {
            "accept": "text/plain",
            "Authorization": "Bearer " + auth_token
          }
        }
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
