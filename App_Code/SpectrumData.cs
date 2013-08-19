using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SpectrumData
/// </summary>
public class SpectrumData
{

    public float mz;
    public float intensity;
    private string color;
    private string tip;

    public SpectrumData()
    {
        this.mz = 0.0f;
        this.intensity = 0.0f;
        this.color = "#000000";
        this.tip = null;
    }

    public SpectrumData(float mz, float intensity)
    {
        this.mz = mz;
        this.intensity = intensity;
        this.color = "#000000";
        this.tip = null;
    }
    public SpectrumData(float mz, float intensity, string color, string tip)
    {
        this.mz = mz;
        this.intensity = intensity;
        this.color = color;
        this.tip = tip;
    }
    public float getMZ()
    {
        return this.mz;
    }
    public float getIntensity()
    {
        return this.intensity;
    }

    public string getColor()
    {
        return this.color;
    }

    public string getTip()
    {
        return this.tip;
    }


    public void setColor(string color)
    {
        this.color = color;
    }

    public void setTip(string tip)
    {
        this.tip = tip;
    }
}