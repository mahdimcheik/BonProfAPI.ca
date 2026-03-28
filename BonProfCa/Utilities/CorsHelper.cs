namespace BonProfCa.Utilities;

public static class CorsHelper
{
    public static bool IsOriginAllowed(string origin)
    {
        List<string> localUrls =
            new()
            {
                    "http://localhost",
                    "https://localhost",
                    "https://localhost:4200",
                    "http://localhost:4200",
                    "https://localhost:4201",
                    "https://192.168.1.19:4201",
                    "http://192.168.1.19:4201",
                    "http://localhost:4201",
                    "http://localhost:7113",
                    "https://localhost:7113",
                    EnvironmentVariables.API_BACK_URL,
                    EnvironmentVariables.API_FRONT_URL,
                    $"api.${EnvironmentVariables.API_BACK_URL}",
                    $"api-dev.${EnvironmentVariables.API_BACK_URL}",
                    $"api-test.${EnvironmentVariables.API_BACK_URL}",
                    $"test.${EnvironmentVariables.API_BACK_URL}",
                    $"dev.${EnvironmentVariables.API_BACK_URL}",
                    "https://recette-tpeweb.e-transactions.fr",
                    "http://www.bonprof.fr",
                    "https://www.bonprof.fr",
                    "http://files.bonprof.fr",
                    "https://files.bonprof.fr",
                    "http://bonprof.fr",
                    "https://bonprof.fr",
            };
        return localUrls.Contains(origin);
    }
}
