using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace RCTool_Client.Packet.Outbound
{
    public class OutboundPacket05DirectoryList : OutboundPacket
    {
        private string directory;

        public OutboundPacket05DirectoryList(string dir) : base(5)
        {
            this.directory = dir;
        }

        public override void WritePacket(BinaryWriter writer)
        {
            writer.Write(directory);
            FileSystemInfo[] entries = null;

            try
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                entries = di.GetFileSystemInfos("*.*");

                uint len = (uint)entries.Length;
                if (di.Parent != null)
                {
                    //write length accounting for extra entry to the top
                    writer.Write((uint)len + 1);
                    writer.Write((byte)3);
                    writer.Write((string)di.Parent.FullName);
                    writer.Write((ulong)0);
                    writer.Write((long)di.Parent.LastWriteTime.Ticks);
                }
                else
                {
                    writer.Write((uint)len);
                }

            }
            catch (Exception ex)
            {
                writer.Write((uint)1);
                writer.Write((byte)2);
                writer.Write(ex.Message);
                writer.Write((ulong)0);
                writer.Write((long)0);
            }

            foreach (var entry in entries ?? new FileSystemInfo[0])
            {
                byte type;
                string name;
                ulong size;
                long ticks;

                try
                {
                    //check if is directory
                    if ((entry.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        type = 0;
                    else
                        type = 1;


                    name = (entry.FullName);

                    //if file, write size, if directory 0 (has no size)
                    if (entry is FileInfo fi)
                        size = ((ulong)fi.Length);
                    else
                        size = ((ulong)0);

                    ticks = (long)(entry.LastWriteTime.Ticks);

                }
                catch (Exception ex)
                {
                    type = ((byte)2);
                    name = (ex.Message);
                    size = ((ulong)0);
                    ticks = ((long)0);
                }


                writer.Write((byte)type);
                writer.Write((string)name);
                writer.Write((ulong)size);
                writer.Write((long)ticks);
            }

        }
    }
}
