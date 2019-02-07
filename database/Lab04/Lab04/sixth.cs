using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;



[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native,
     IsByteOrdered = true)]
public struct Types : INullable
{
    private bool is_Null;
    private Int32 _serials;
    private Int32 _films;
    private Int32 _anounce;

    public bool IsNull
    {
        get
        {
            return (is_Null);
        }
    }

    public static Types Null
    {
        get
        {
            Types v = new Types();
            v.is_Null = true;
            return v;
        }
    }
 
    public override string ToString()
    {

        if (this.IsNull)
            return "NULL";
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("serials:" + _serials);
            builder.Append(", films:" + _films);
            builder.Append(", anounce:" + _anounce);

            return builder.ToString();
        }
    }

    
    [SqlMethod(OnNullCall = false)]
    public static Types Parse(SqlString s)
    {
        Types types = new Types();
        string[] get = s.Value.Split(",".ToCharArray());
        types._serials = Int32.Parse(get[0]);
        types._films = Int32.Parse(get[1]);
        types._anounce = Int32.Parse(get[2]);

        return types;
    }
    


    public Int32 Serials
    {
        get
        {
            return this._serials;
        }
 
        set
        {
            _serials = value;
        }
    }

    public Int32 Fillms
    {
        get
        {
            return this._films;
        }
        set
        {
            _films = value;
        }
    }

    public Int32 Anounce
    {
        get
        {
            return this._anounce;
        }
        set
        {
            _anounce = value;
        }
    }

    [SqlMethod(OnNullCall = false)]
    public Double Summ()
    {
        return (_serials + _films + _anounce);
    }

}