module Hw6.Client.CalculatorRequestClient

open System
open System.Globalization
open System.Net
open System.Net.Http
open System.Threading.Tasks


type MathExpression = {
    value1: decimal
    operation: string
    value2: decimal
}

type ErrorInfo =
    { code: int
      message: string }
    override this.ToString() =
        $"Error: %d{int this.code} %s{this.message}"

type Response<'a> = Result<'a, ErrorInfo>

type CalculatorRequestClient(url: string) =
    let httpClient = new HttpClient()
    
    member this.Calculate data : Task<Response<string>> =
        task {
            try
                let val1 = data.value1.ToString(CultureInfo.InvariantCulture)
                let val2 = data.value2.ToString(CultureInfo.InvariantCulture)
                let! calculation =
                    httpClient.GetAsync( $"{url}/calculate?value1={val1}&operation={data.operation}&value2={val2}")
                let! response = calculation.Content.ReadAsStringAsync()
                match calculation.StatusCode with
                | HttpStatusCode.OK -> return Ok response
                | _ ->
                    return
                        Error
                            { code = int calculation.StatusCode
                              message = response}
            with
            | ex -> return Error { code = -1; message = ex.Message }
        }
    interface IDisposable with
      member __.Dispose() = httpClient.Dispose()
