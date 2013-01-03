using System;

using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.BinaryFiles.IwGx
{
	public class MaterialBinarySerializer : IBinarySerializer
	{
		private readonly IComponentContext context;

		public MaterialBinarySerializer(IComponentContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Parse binary block.
		/// </summary>
		public Managed Parse(BinaryParser parser)
		{
			var material = context.Resolve<Material>();
			material.NameHash = parser.ConsumeUInt32();

			var isShort = parser.ConsumeBool();

			if (!isShort)
			{
				material.ZDepthOfs = parser.ConsumeInt32();
				material.ZDepthOfsHW = parser.ConsumeInt32();
				material.ColEmissive = parser.ConsumeColor();
				material.ColAmbient = parser.ConsumeColor();
				material.ColDiffuse = parser.ConsumeColor();
				material.ColSpecular = parser.ConsumeColor();
				material.SpecularPower = material.ColSpecular.A;
				parser.Expect((uint)4);
			}
			material.Texture0.HashReference = parser.ConsumeUInt32();
			if (!isShort)
			{
				material.Texture1.HashReference = parser.ConsumeUInt32();
				parser.ConsumeUInt32(); ///TODO: probably shader

				var animated = parser.ConsumeBool();
				if (animated)
				{
					material.MatAnim = new MatAnim();
					parser.Expect((byte)0);
					material.MatAnim.CelNum = parser.ConsumeByte();
					material.MatAnim.CelNumU = parser.ConsumeByte();
					material.MatAnim.CelW = parser.ConsumeByte();
					material.MatAnim.CelH = parser.ConsumeByte();
					material.MatAnim.CelPeriod = parser.ConsumeByte();
				}
				material.AlphaTestValue = parser.ConsumeByte();
				parser.ConsumeUInt32(); ///TODO: probably shader
				parser.ConsumeUInt32(); ///TODO: probably shader
			}
			return material;
		}



	}
}