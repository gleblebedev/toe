﻿using System;

using Autofac;

using Toe.Marmalade.IwAnim;
using Toe.Utils;

namespace Toe.Marmalade.BinaryFiles.IwAnim
{
	public class AnimBinarySerializer : IBinarySerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public AnimBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var anim = this.context.Resolve<Anim>();
			anim.NameHash = parser.ConsumeUInt32();

			anim.Skeleton.HashReference = parser.ConsumeUInt32();
			var numBones = parser.ConsumeUInt32();
			var boneFlags = parser.ConsumeUInt32();

			for (var numFrames = parser.ConsumeUInt32(); numFrames > 0; --numFrames)
			{
				var frameId = parser.ConsumeUInt32();
				if (frameId == Hash.Get("CIwAnimKeyFrame"))
				{
					var frame = this.context.Resolve<AnimKeyFrame>();
					parser.Expect((uint)0x0);
					frame.Time = parser.ConsumeFloat();
					var type = parser.ConsumeByte();
					var someVec = parser.ConsumeVector3();
					parser.Expect(0x01);

					anim.AddFrame(frame);

					uint num;
					bool b;
					switch (type)
					{
						case 2:
							parser.ConsumeUInt32(); // 0x01FFFFF, 0x01FFFFE
							num = parser.ConsumeUInt32();
							b = parser.ConsumeBool();

							for (uint index = 0; index < num; ++index)
							{
								var bone = frame.Bones[(int)index];
								bone.BindingPos = parser.ConsumeVector3();
								bone.BindingRot = parser.ConsumeQuaternion();
							}
							break;
						case 3:
							parser.ConsumeUInt32(); // 0x00002000, 1
							num = parser.ConsumeUInt32();
							b = parser.ConsumeBool();
							for (uint index = 0; index < num; ++index)
							{
								parser.ConsumeQuaternion();
							}
							break;
						default:
							throw new NotImplementedException();
					}
					continue;
				}
				throw new NotImplementedException();

				//this.m_KeyFrames.Serialise(serialise);
			}
			anim.Duration = parser.ConsumeFloat();
			var aaa = parser.ConsumeUInt32();
			return anim;
			//throw new NotImplementedException();

			//serialise.Fixed(ref this.m_TransformPrecision);
			//serialise.ManagedHash(ref this.m_OfsAnim);
			//serialise.DebugWrite(256);
		}

		#endregion
	}
}