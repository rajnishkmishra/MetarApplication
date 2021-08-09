using System;
using MetarApplication.Models;
using MetarApplication.Services.Common;

namespace MetarApplication.Services.Mapper
{
	public interface IResponseMapper
	{
		Result MapResponse(string response);
	}

    public class ResponseMapper : IResponseMapper
    {

    	public Result MapResponse(string response)
        {
            string[] splittedResponse = response.Split(' ');
            string[] timeAndStation = splittedResponse[1].Split('\n');

            return new Result()
            {
                data = new Data()
                {
                    station = timeAndStation[1],
                    last_observation = String.Format("{0} at {1} GMT",splittedResponse[0],timeAndStation[0]),
                    temperature = MapTemperature(splittedResponse),
                    wind = splittedResponse[3]=="AUTO" ? MapWind(splittedResponse[4]) : MapWind(splittedResponse[3])
                }
            };
        }

        private string MapTemperature(string[] s)
        {
            int i=6;
            for(;i<s.Length;i++)
            {
                if(s[i].Contains("/"))
                    break;
            }
            string temperature = s[i].Split('/')[0];

            if(temperature[0]=='M')
            return String.Format("- {0} C ({1} F)",Convert.ToInt32(temperature.Substring(1)), GetTemperatureInFahrenheit(temperature.Substring(1)));

            return String.Format("{0} C ({1} F)",Convert.ToInt32(temperature.Substring(0)), GetTemperatureInFahrenheit(temperature.Substring(0)));
        }

        private string GetTemperatureInFahrenheit(string s)
        {
            double temperature = Convert.ToDouble(s);
            temperature = temperature * (9/5.0) + 32;
            return temperature.ToString();
        }

        private string MapWind(string s)
        {
            string windSpeedInKnots, windDirectionInDegree;
            int wind;
            if(s.Contains("G"))
            {
                string[] windInfo = s.Split('G');
                windDirectionInDegree = windInfo[0].Substring(0,3);
                windSpeedInKnots = windInfo[1].Split('K')[0];
                wind = Convert.ToInt32(windSpeedInKnots);        
            }
            else
            {
                string windInfo = s.Split('K')[0];
                windDirectionInDegree = windInfo.Substring(0,3);
                windSpeedInKnots = windInfo.Substring(3);
                wind = Convert.ToInt32(windSpeedInKnots);
            }
            if(windDirectionInDegree.Contains("VRB"))
                return String.Format("{0} mph ({1} knots)",Math.Round(wind*1.15077945),wind.ToString());
            if(windDirectionInDegree=="000" && wind==0)
                return "CALM";
            return String.Format("{0} at {1} mph ({2} knots)",GetWindDirection(Convert.ToDouble(windDirectionInDegree)),Math.Round(wind*1.15077945),wind.ToString());
        }

        private string GetWindDirection(double windDirectionInDegree)
        {
            windDirectionInDegree = windDirectionInDegree % 360;
            int index = Convert.ToInt32(windDirectionInDegree / 22.5);
            return Constants.windDirection[index];
        }
    }
}