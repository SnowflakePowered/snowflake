using System.IO;
using System.Text;

namespace Snowflake.Plugin.FileSignatures.SFOSharp
{
    public sealed class SFOKeyTableEntry
    {
	    public static byte delimiterByte = 0;
	    private int keyTableLength;
	
	    public SFOKeyTableEntry() {
		    this.keyTableLength = 0;
	    }
	
	    /**
	     * Reads a key from the keyTable and return the value
	     * 
	     * @param fIn
	     * @return String
	     * @throws IOException
	     */
	    public string ReadEntry(Stream fIn) {
		    byte[] tempByteArray1 = new byte[1];
		    StringBuilder sb = new StringBuilder();
		
		    fIn.Read(tempByteArray1, 0, 1);
	        this.keyTableLength++;
		    while(tempByteArray1[0] != SFOKeyTableEntry.delimiterByte) {
			    sb.Append((char)tempByteArray1[0]);
			    fIn.Read(tempByteArray1, 0, 1);
		        this.keyTableLength++;
		    }
		
		    return sb.ToString();
	    }
	
	    /**
	     * Returns the keyTable-length in bytes
	     * @return Integer
	     */
	    public int GetKeyTableLength() {
		    return this.keyTableLength;
	    }
    }
}
