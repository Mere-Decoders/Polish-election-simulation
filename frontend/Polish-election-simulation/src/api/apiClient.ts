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

  // All methods can be static and if needed acess the data in the singleton by using getInstance()
  // Making them static makes the invokation cleaner (you don't need to use the getInstance() method)
  public static getApportionmentMethodIDs(): ApportionmentMethodID[] {
    let result: ApportionmentMethodID[] = [
        new ApportionmentMethodID("D'Hondta"),
        new ApportionmentMethodID("Sainte-Lague")
    ];
    return result;
  }

  public static getVotesIDs(): VotesID[] {
    let result: VotesID[] = [
        new VotesID("2019"),
        new VotesID("2023")
    ];
    return result;
  }

  public static getConstituencySetIDs(): ConstituencySetID[] {
    let result: ConstituencySetID[] = [
        new ConstituencySetID("Oficjalne"),
        new ConstituencySetID("Zestaw Lorem ipsum")
    ];
    return result;
  }

  public async getTotalResults(year: number, method: string): Promise<ResultsTableRow[]>  {
    //const data_response = await fetch("/mock_results.json");
      const backend_address = "https://polishelectionsimulation-dnevb2c4fse7dwc6.polandcentral-01.azurewebsites.net"
     const auth = await fetch(
        backend_address + "/api/Auth/login",
         {
             method: "POST",
             headers: {
                 "accept": "*/*",
                 "Content-Type": "application/json"
             },
             body: JSON.stringify({username: "string", password: "string"})
         }
     );
      const data_response = await fetch(
          backend_address + "/api/Simulation?simDataGuid=00000000-0000-0000-0000-000000000001&methodGuid=00000000-0000-0000-0000-000000000001",
         {
             headers: {
                 "accept": "text/plain",
                 "Authorization": "Bearer " + (await auth.json()).token
             }
         }
     );
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
