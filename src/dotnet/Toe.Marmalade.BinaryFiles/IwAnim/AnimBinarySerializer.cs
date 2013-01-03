using System;
using System.Globalization;

using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class AnimBinarySerializer:IBinarySerializer
	{
		private readonly IComponentContext context;

		public AnimBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var anim = context.Resolve<Anim>();
			anim.NameHash = parser.ConsumeUInt32();

			anim.Skeleton.HashReference = parser.ConsumeUInt32();
			var numBones = parser.ConsumeUInt32();
			var boneFlags = parser.ConsumeUInt32();

			for (var numFrames = parser.ConsumeUInt32();numFrames > 0;--numFrames)
			{
				var frameId = parser.ConsumeUInt32();
				throw new NotImplementedException();

				//this.m_KeyFrames.Serialise(serialise);
			}
			throw new NotImplementedException();

			anim.Duration = parser.ConsumeFloat();
			//serialise.Fixed(ref this.m_TransformPrecision);
			//serialise.ManagedHash(ref this.m_OfsAnim);
			//serialise.DebugWrite(256);
		}

	
	}
}
