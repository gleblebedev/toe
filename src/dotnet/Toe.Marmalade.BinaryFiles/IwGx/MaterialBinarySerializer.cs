using System.Drawing;

using Autofac;

using Toe.Marmalade.IwGx;
using Toe.Utils.Marmalade.IwGx;

namespace Toe.Marmalade.BinaryFiles.IwGx
{
	public class MaterialBinarySerializer : IBinarySerializer
	{
		#region Constants and Fields

		private readonly IComponentContext context;

		#endregion

		#region Constructors and Destructors

		public MaterialBinarySerializer(IComponentContext context)
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
			var material = this.context.Resolve<Material>();
			material.NameHash = parser.ConsumeUInt32();

			var isShort = parser.ConsumeBool();

			material.Flags = parser.ConsumeUInt32();

			if (!isShort)
			{
				material.ZDepthOfs = parser.ConsumeInt16();
				material.ZDepthOfsHW = parser.ConsumeInt16();
				material.ColEmissive = parser.ConsumeColor();
				material.ColAmbient = parser.ConsumeColor();
				material.ColDiffuse = parser.ConsumeColor();
				Color specular = parser.ConsumeColor();
				material.ColSpecular = specular;
				material.SpecularPower = specular.A;
				parser.Expect((uint)4);
			}
			material.Texture0.HashReference = parser.ConsumeUInt32();
			if (!isShort)
			{
				material.Texture1.HashReference = parser.ConsumeUInt32();
				material.Texture2.HashReference = parser.ConsumeUInt32();
				material.Texture3.HashReference = parser.ConsumeUInt32();

				var animated = parser.ConsumeBool();
				if (animated)
				{
					material.MatAnim = new MatAnim();
					parser.Expect(0);
					material.MatAnim.CelNum = parser.ConsumeByte();
					material.MatAnim.CelNumU = parser.ConsumeByte();
					material.MatAnim.CelW = parser.ConsumeByte();
					material.MatAnim.CelH = parser.ConsumeByte();
					material.MatAnim.CelPeriod = parser.ConsumeByte();
				}
				material.AlphaTestValue = parser.ConsumeByte();
				material.ShaderTechnique.HashReference = parser.ConsumeUInt32();
			}
			return material;
		}

		#endregion
	}
}