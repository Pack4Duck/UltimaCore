﻿using System;
using System.Collections.Generic;
using System.Text;
using UltimaCore.Graphics;

namespace UltimaCore
{
    public class UOFileMul : UOFile
    {
        private readonly UOFileIdxMul _idxFile;
        private readonly int _patch;

        public UOFileMul(string file, string idxfile, int patch = -1) : base(file)
        {
            _idxFile = new UOFileIdxMul(idxfile);
            _patch = patch;
            Load();
        }

        public UOFileMul(string file) : base(file)
        {
            Load();
        }

        protected override void Load()
        {
            base.Load();

            if (_idxFile != null)
            {

                int count = (int)_idxFile.Length / 12;

                Entries3D = new UOFileIndex3D[count];

                for (int i = 0; i < count; i++)
                    Entries3D[i] = new UOFileIndex3D(_idxFile.ReadInt(), _idxFile.ReadInt(), _idxFile.ReadInt());

                var patches = Verdata.Patches;

                for (int i = 0; i < patches.Length; i++)
                {
                    var patch = patches[i];

                    if (patch.File == _patch && patch.Index >= 0 &&
                        patch.Index < Entries3D.Length)
                    {
                        UOFileIndex3D entry = Entries3D[patch.Index];
                        entry.Offset = patch.Offset;
                        entry.Length = patch.Length | (1 << 31);
                        entry.Extra = patch.Extra;
                    }
                }
            }
        }

        private class UOFileIdxMul : UOFile
        {
            public UOFileIdxMul(string idxpath) : base(idxpath) { }
        }
    }
}
