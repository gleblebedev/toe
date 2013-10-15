using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace Toe.Utils.ToeMath.Tests
{
	[TestFixture]
	public class CodeGen
	{
		#region Constants and Fields

		private readonly string[] vectorComponents = new[] { "X", "Y", "Z", "W" };

		private readonly string[] vectorSubComponents = new[] { "", "Xy", "Xyz" };

		#endregion

		#region Public Methods and Operators

		[Test]
		public void GenerateSourceCode()
		{
			TypeInfo[] types = new[]
				{
					new TypeInfo("float", 0, 1), new TypeInfo("int", 0, 1), new TypeInfo("uint", 0, 1),
					new TypeInfo("double", 0, 1) { ComponentSize = 8 }, new TypeInfo("half", 0, 1) { ComponentSize = 2 },
				};
			string basePath = @"C:\GitHub\toe\src\dotnet\NuGet\Toe.Utils.ToeMath\";
			foreach (var baseType in types)
			{
				for (int rows = 0; rows <= 4; ++rows)
				{
					for (int cols = 1; cols <= 4; ++cols)
					{
						this.GenSource(basePath, new TypeInfo(baseType.BaseName, rows, cols){ComponentSize = baseType.ComponentSize});
					}
				}
			}
		}

		#endregion

		#region Methods

		private static void GenHash(StreamWriter stream, IEnumerable<string> members)
		{
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Returns the hash code for this instance.");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <returns>");
			stream.WriteLine("\t\t/// A 32-bit signed integer that is the hash code for this instance.");
			stream.WriteLine("\t\t/// </returns>");
			stream.WriteLine("\t\t/// <filterpriority>2</filterpriority>");
			stream.WriteLine("\t\tpublic override int GetHashCode()");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tunchecked");
			stream.WriteLine("\t\t\t{");

			var enumerator = members.GetEnumerator();
			enumerator.MoveNext();
			stream.WriteLine("\t\t\t\tint hashCode = this." + enumerator.Current + ".GetHashCode();");
			while (enumerator.MoveNext())
			{
				stream.WriteLine("\t\t\t\thashCode = (hashCode * 397) ^ this." + enumerator.Current + ".GetHashCode();");
			}
			stream.WriteLine("\t\t\t\treturn hashCode;");
			stream.WriteLine("\t\t\t}");
			stream.WriteLine("\t\t}");
		}

		private void GenMatrixFields(TypeInfo type, StreamWriter stream)
		{
			for (int i = 0; i < type.Rows; ++i)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Row of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				var rowOffset = i * type.ComponentSize * type.Cols;
				//stream.WriteLine("\t\t[FieldOffset(" + rowOffset + ")]");
				//stream.WriteLine("\t\tpublic " + type.BaseName + type.Cols + " Row" + (i) + ";");
				stream.Write("\t\tpublic " + type.BaseName + type.Cols + " Row" + i + " { get { return new "+type.BaseName+type.Cols+"(");

				for (int j = 0; j < type.Cols; ++j) stream.Write("{2}this.M{0}{1}", i, j, j == 0?"":", ");
				stream.WriteLine("); } set {"+ new Numbers(0,type.Cols-1).Select(col=> string.Format("this.M{0}{1} = value.{2};",i,col,vectorComponents[col])).StringJoin("")+"} }");

				for (int j = 0; j < type.Cols; ++j)
				{
					stream.WriteLine("");
					stream.WriteLine("\t\t/// <summary>");
					stream.WriteLine("\t\t/// Row " + i + ", Column " + j + " of the " + type.Name + ".");
					stream.WriteLine("\t\t/// </summary>");
					stream.WriteLine("\t\t[FieldOffset(" + (rowOffset + j * type.ComponentSize) + ")]");
					stream.WriteLine("\t\tpublic " + type.ClrName + " M" + i + j + ";");
				}
			}

				stream.WriteLine("");
				stream.WriteLine(
					"\t\tpublic static readonly " + type.Name + " Identity = new " + type.Name + "("
					+ new Numbers(0, type.Rows - 1).Select(row => new Numbers(0, type.Cols - 1).Select(col=>(col==row)?"1":"0").StringJoin(", ")).StringJoin(", ")+");");
		}

		private void GenEqualityMembers(TypeInfo type, StreamWriter stream, IEnumerable<string> members)
		{
			GenHash(stream, members);
			stream.WriteLine(
				"\t\tpublic static bool operator ==(" + type.Name + " left, " + type.Name + " right) { return left.Equals(right); }");
			stream.WriteLine(
				"\t\tpublic static bool operator !=(" + type.Name + " left, " + type.Name
				+ " right) { return !left.Equals(right); }");

			stream.WriteLine("");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Indicates whether the current object is equal to another object of the same type.");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <returns>");
			stream.WriteLine(
				"\t\t/// true if the current object is equal to the <paramref name=\"other\"/> parameter; otherwise, false.");
			stream.WriteLine("\t\t/// </returns>");
			stream.WriteLine("\t\t/// <param name=\"other\">An object to compare with this object.</param>");
			stream.WriteLine("\t\tpublic bool Equals(" + type.Name + " other)");
			stream.WriteLine("\t\t{");
			stream.WriteLine(
				"\t\t\treturn " + members.Select(x => "this." + x + ".Equals(other." + x + ")").StringJoin(" && ") + ";");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Indicates whether this instance and a specified object are equal.");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <returns>");
			stream.WriteLine(
				"\t\t/// true if <paramref name=\"obj\"/> and this instance are the same type and represent the same value; otherwise, false.");
			stream.WriteLine("\t\t/// </returns>");
			stream.WriteLine(
				"\t\t/// <param name=\"obj\">Another object to compare to. </param><filterpriority>2</filterpriority>");
			stream.WriteLine("\t\tpublic override bool Equals(object obj)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tif (ReferenceEquals(null, obj))");
			stream.WriteLine("\t\t\t{");
			stream.WriteLine("\t\t\t\treturn false;");
			stream.WriteLine("\t\t\t}");
			stream.WriteLine("\t\t\treturn obj is Float4 && Equals((Float4)obj);");
			stream.WriteLine("\t\t}");
		}

		private void GenMatrixCtors(TypeInfo type, StreamWriter stream)
		{
			stream.WriteLine("");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Constructor of the " + type.Name + ".");
			stream.WriteLine("\t\t/// </summary>");
			stream.Write("\t\tpublic " + type.Name + "(");
			string sep = "";
			for (int i = 0; i < type.Rows; ++i )
				for (int j = 0; j < type.Cols; j++)
				{
					stream.Write(sep);
					stream.Write(type.ClrName);
					stream.Write(" m");
					stream.Write(i);
					stream.Write(j);
					sep = ", ";
				}
					stream.WriteLine(" )");
			stream.WriteLine("\t\t{");
			for (int i = 0; i < type.Rows; ++i)
				for (int j = 0; j < type.Cols; j++)
				{
					stream.Write("\t\t\t");
					stream.Write("this.M");
					stream.Write(i);
					stream.Write(j);
					stream.Write(" = m");
					stream.Write(i);
					stream.Write(j);
					stream.WriteLine(";");
				}
			stream.WriteLine("\t\t}");
		}

		private void GenVectorCtors(TypeInfo type, StreamWriter stream)
		{
			stream.WriteLine("");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Constructor of the " + type.Name + ".");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tpublic " + type.Name + "(" + type.ClrName + " scale)");
			stream.WriteLine("\t\t{");
			this.vectorComponents.Take(type.Cols).Select(x => "\t\t\tthis." + x + " = scale;").ForEach(stream.WriteLine);
			stream.WriteLine("\t\t}");
			if (type.Cols > 1)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Constructor of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				var vectorComponent = this.vectorComponents[type.Cols - 1];
				stream.WriteLine(
					"\t\tpublic " + type.Name + "(" + type.BaseName + (type.Cols - 1) + " vec, " + type.ClrName + " " + vectorComponent
					+ ")");
				stream.WriteLine("\t\t{");
				this.vectorComponents.Take(type.Cols-1).Select(x => "\t\t\tthis." + x + " = vec." + x + ";").ForEach(stream.WriteLine);
				stream.WriteLine("\t\t\tthis." + vectorComponent + " = " + vectorComponent + ";");
				stream.WriteLine("\t\t}");
				
			}
			for (int i = 2; i <= type.Cols; ++i)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Constructor of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine(
					"\t\tpublic " + type.Name + "("
					+ this.vectorComponents.Take(i).Select(x => type.ClrName + " " + x).StringJoin(", ") + ")");
				stream.WriteLine("\t\t{");
				this.vectorComponents.Take(i).Select(x => "\t\t\tthis." + x + " = " + x + ";").ForEach(stream.WriteLine);
				this.vectorComponents.Skip(i).Take(type.Cols - i).Select(
					x => "\t\t\tthis." + x + " = default(" + type.ClrName + ");").ForEach(stream.WriteLine);
				stream.WriteLine("\t\t}");
			}

			for (int i = 1; i <= type.Cols; ++i)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Constructor of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\tpublic " + type.Name + "(" + type.BaseName + i + " a)");
				stream.WriteLine("\t\t{");
				this.vectorComponents.Take(i).Select(x => "\t\t\tthis." + x + " = a." + x + ";").ForEach(stream.WriteLine);
				this.vectorComponents.Skip(i).Take(type.Cols - i).Select(
					x => "\t\t\tthis." + x + " = default(" + type.ClrName + ");").ForEach(stream.WriteLine);
				stream.WriteLine("\t\t}");
			}
		}


		private void GenVectorMethods(TypeInfo type, StreamWriter stream)
		{
			stream.WriteLine("\t\tpublic void Normalize()");
			stream.WriteLine("\t\t{");
			if (type.ClrName == "float")
			{
				stream.WriteLine("\t\t\t"+type.ClrName+" scale = 1.0f/this.Length;");
				for (int j = 0; j < type.Cols ; ++j)
				{
					stream.Write("\t\t\t");
					stream.Write(this.vectorComponents[j]);
					stream.WriteLine(" *= scale;");
				}
			}
			else
			{
				stream.WriteLine("\t\t\t" + type.ClrName + " len = this.Length;");
				for (int j = 0; j < type.Cols; ++j)
				{
					stream.Write("\t\t\t");
					stream.Write(this.vectorComponents[j]);
					stream.WriteLine(" /= len;");
				}
			}
			stream.WriteLine("\t\t}");


			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static " + type.Name + " Multiply (" + type.Name + " left, " + type.Name + " right)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\treturn new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right.{0})").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static " + type.Name + " Multiply (" + type.Name + " left, " + type.ClrName + " right)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\treturn new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right)").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static void Multiply (ref " + type.Name + " left, ref " + type.Name + " right, out " + type.Name + " result)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tresult = new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right.{0})").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static void Add (ref " + type.Name + " left, ref " + type.Name + " right, out " + type.Name + " result)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tresult = new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} + right.{0})").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static void Sub (ref " + type.Name + " left, ref " + type.Name + " right, out " + type.Name + " result)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tresult = new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} - right.{0})").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static void Multiply (ref " + type.Name + " left, " + type.ClrName + " right, out " + type.Name + " result)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tresult = new " + type.Name + "(" +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right)").StringJoin(", ")
				+ ");");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static " + type.ClrName + " Dot (" + type.Name + " left, " + type.Name + " right)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\treturn " +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right.{0})").StringJoin(" + ")
				+ ";");
			stream.WriteLine("\t\t}");

			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static " + type.ClrName + " Dot (ref " + type.Name + " left, ref " + type.Name + " right)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\treturn " +
				vectorComponents.Take(type.Cols).StringFormat("(left.{0} * right.{0})").StringJoin(" + ")
				+ ";");
			stream.WriteLine("\t\t}");


			stream.WriteLine("\t\tpublic static " + type.Name + " Normalize(" + type.Name + " vec)");
			stream.WriteLine("\t\t{");
			if (type.ClrName == "float")
			{
				stream.WriteLine("\t\t\t" + type.ClrName + " scale = 1.0f/vec.Length;");
				for (int j = 0; j < type.Cols; ++j)
				{
					stream.Write("\t\t\tvec.");
					stream.Write(this.vectorComponents[j]);
					stream.WriteLine(" *= scale;");
				}
				stream.WriteLine("\t\t\treturn vec;");
			}
			else
			{
				stream.WriteLine("\t\t\t" + type.ClrName + " len = vec.Length;");
				for (int j = 0; j < type.Cols; ++j)
				{
					stream.Write("\t\t\tvec.");
					stream.Write(this.vectorComponents[j]);
					stream.WriteLine(" /= len;");
				}
				stream.WriteLine("\t\t\treturn vec;");
			}
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\tpublic static " + type.Name + " operator -(" + type.Name + " left, " + type.Name + " right)");
			stream.WriteLine("\t\t{");
			for (int j = 0; j < type.Cols; ++j)
			{
				stream.Write("\t\t\tleft.");
				stream.Write(this.vectorComponents[j]);
				stream.Write(" -= right.");
				stream.Write(this.vectorComponents[j]);
				stream.WriteLine(";");
			}
			stream.WriteLine("\t\t\treturn left;");
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\tpublic static " + type.Name + " operator +(" + type.Name + " left, " + type.Name + " right)");
			stream.WriteLine("\t\t{");
			for (int j = 0; j < type.Cols; ++j)
			{
				stream.Write("\t\t\tleft.");
				stream.Write(this.vectorComponents[j]);
				stream.Write(" += right.");
				stream.Write(this.vectorComponents[j]);
				stream.WriteLine(";");
			}
			stream.WriteLine("\t\t\treturn left;");
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\tpublic static " + type.Name + " operator *(" + type.Name + " left, " + type.ClrName + " scale)");
			stream.WriteLine("\t\t{");
			for (int j = 0; j < type.Cols; ++j)
			{
				stream.Write("\t\t\tleft.");
				stream.Write(this.vectorComponents[j]);
				stream.WriteLine(" *= scale;");
			}
			stream.WriteLine("\t\t\treturn left;");
			stream.WriteLine("\t\t}");
		}

		private void GenVectorFields(TypeInfo type, StreamWriter stream)
		{
			for (int j = 1; j < type.Cols - 1; ++j)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// The " + this.vectorSubComponents[j] + " component of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\tpublic " + type.BaseName + (j + 1) + " " + this.vectorSubComponents[j] + " {");
				stream.Write("\t\t\tget { return new " + type.BaseName + (j + 1) + "(this." + this.vectorComponents[0]);

				for (int i = 1; i <= j; ++i)
				{
					stream.Write(", this.");
					stream.Write(this.vectorComponents[i]);
				}
				stream.WriteLine("); }");
				stream.Write("\t\t\tset { ");

				for (int i = 0; i <= j; ++i)
				{
					stream.Write("this.");
					stream.Write(this.vectorComponents[i]);
					stream.Write(" = value.");
					stream.Write(this.vectorComponents[i]);
					stream.Write("; ");
				}
				stream.WriteLine(" }");
				stream.WriteLine("\t\t}");
			}
			for (int i = 0; i < type.Cols; i++)
			{
				stream.WriteLine("");
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// The " + this.vectorComponents[i] + " component of the " + type.Name + ".");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\t[FieldOffset(" + i * type.ComponentSize + ")]");
				stream.WriteLine("\t\tpublic " + type.ClrName + " " + this.vectorComponents[i] + ";");
			}
			for (int i = 0; i < type.Cols; i++)
			{
				stream.WriteLine("");
				stream.WriteLine(
					"\t\tpublic static readonly " + type.Name + " Unit" + this.vectorComponents[i] + " = new " + type.Name + "("
					+ this.vectorComponents.Take(type.Cols).SelectIndex((j, x) => (j == i) ? "1" : "0").StringJoin(", ") + ");");
			}
			stream.WriteLine("");
			stream.WriteLine(
				"\t\tpublic static readonly " + type.Name + " Zero = new " + type.Name + "("
				+ this.vectorComponents.Take(type.Cols).Select(x => "0").StringJoin(", ") + ");");
			stream.WriteLine("");
			stream.WriteLine("\t\tpublic static readonly int SizeInBytes = Marshal.SizeOf(new " + type.Name + "());");

			stream.WriteLine("");
			stream.WriteLine(
				"\t\tpublic " + type.ClrName + " Length { get { return (" + type.ClrName + ")Math.Sqrt("
				+ this.vectorComponents.Take(type.Cols).Select(x => "(this." + x + " * this." + x + ")").StringJoin(" + ")
				+ "); } }");

			stream.WriteLine("");
			stream.WriteLine(
				"\t\tpublic " + type.ClrName + " LengthSquared { get { return "
				+ this.vectorComponents.Take(type.Cols).Select(x => "(this." + x + " * this." + x + ")").StringJoin(" + ")
				+ "; } }");
		}

		private void GenSource(string basePath, TypeInfo type)
		{
			var file = Path.Combine(basePath, type.Name) + ".cs";
			using (var fileStream = File.Open(file, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (var stream = new StreamWriter(fileStream, Encoding.UTF8))
				{
					stream.WriteLine("using System;");
					stream.WriteLine("using System.Runtime.InteropServices;");
					stream.WriteLine("");
					stream.WriteLine("namespace Toe.Utils.ToeMath");
					stream.WriteLine("{");
					stream.WriteLine("#if !WINDOWS_PHONE");
					stream.WriteLine("\t[Serializable]");
					stream.WriteLine("#endif");
					stream.WriteLine("\t[StructLayout(LayoutKind.Explicit)]");
					stream.WriteLine("\tpublic partial struct " + type.Name + ": IEquatable<" + type.Name + ">");
					stream.WriteLine("\t{");
					if (type.Rows == 0)
					{
						this.GenVectorCtors(type, stream);
						this.GenVectorFields(type, stream);
						this.GenVectorMethods(type, stream);

						this.GenEqualityMembers(type, stream, this.vectorComponents.Take(type.Cols).ToArray());
						this.GenToString(type, stream, this.vectorComponents.Take(type.Cols).ToArray());

						stream.WriteLine("\t}");

						stream.WriteLine("\tpublic static partial class MathHelper");
						stream.WriteLine("\t{");

					


						stream.WriteLine("");
						stream.WriteLine("\t\tpublic static " + type.Name + " Scale (" + type.Name + " left, " + type.ClrName + " scale)");
						stream.WriteLine("\t\t{");
						stream.WriteLine("\t\t\treturn new " + type.Name + "(" +
							vectorComponents.Take(type.Cols).StringFormat("(left.{0} * scale)").StringJoin(", ")
							+ ");");
						stream.WriteLine("\t\t}");

					

						stream.WriteLine("");

						stream.WriteLine("\t}");
					}
					else
					{
						GenMatrixCtors(type, stream);
						GenMatrixFields(type, stream);
						this.GenEqualityMembers(type, stream, new MatrixEnumerable(type.Rows, type.Cols));
						this.GenToString(type, stream, (new MatrixEnumerable(type.Rows, type.Cols)).ToArray());
						stream.WriteLine("\t}");

						stream.WriteLine("\tpublic static partial class MathHelper");
						stream.WriteLine("\t{");
						stream.WriteLine("\t}");
					}

					stream.WriteLine("}");
				}
			}
		}

		private void GenToString(TypeInfo type, StreamWriter stream, IList<string> p2)
		{
			stream.WriteLine("");
			stream.WriteLine("\t\tpublic override string ToString() { return string.Format(\"("+
				p2.SelectIndex((i,x)=>"{"+i+"}").StringJoin(", ")
				+ ")\", "+p2.StringFormat("this.{0}").StringJoin(", ")+"); }");
		}

		#endregion

		internal class TypeInfo
		{
			#region Constants and Fields

			private readonly string baseName;

			private readonly string clrName;

			private readonly int cols;

			private readonly string name;

			private readonly int rows;

			private int componentSize = 4;

			#endregion

			#region Constructors and Destructors

			public TypeInfo(string name, int rows, int cols)
			{
				this.rows = rows;
				this.cols = cols;
				this.baseName = char.ToUpper(name[0]) + name.Substring(1);
				this.clrName = char.ToLower(name[0]) + name.Substring(1);
				if (rows == 0)
				{
					this.name = this.BaseName + cols;
				}
				else
				{
					this.name = this.BaseName + rows + "x" + cols;
				}
			}

			#endregion

			#region Public Properties

			public string BaseName
			{
				get
				{
					return this.baseName;
				}
			}

			public string ClrName
			{
				get
				{
					return this.clrName;
				}
			}

			public int Cols
			{
				get
				{
					return this.cols;
				}
			}

			public int ComponentSize
			{
				get
				{
					return this.componentSize;
				}
				set
				{
					this.componentSize = value;
				}
			}

			public string Name
			{
				get
				{
					return this.name;
				}
			}

			public int Rows
			{
				get
				{
					return this.rows;
				}
			}

			#endregion
		}
	}

	internal class MatrixEnumerable : IEnumerable<string>
	{
		#region Constants and Fields

		private readonly int cols;

		private readonly int rows;

		#endregion

		#region Constructors and Destructors

		public MatrixEnumerable(int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<string> GetEnumerator()
		{
			for (int i = 0; i < this.rows; i++)
			{
				for (int j = 0; j < this.cols; j++)
				{
					yield return "M" + i + j;
				}
			}
		}

		#endregion

		#region Explicit Interface Methods

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}

	public static class ExtensionMethods
	{
		#region Public Methods and Operators

		public static void ForEach<T>(this IEnumerable<T> data, Action<T> action)
		{
			foreach (var a in data)
			{
				action(a);
			}
		}

		public static IEnumerable<T2> SelectIndex<T, T2>(this IEnumerable<T> data, Func<int, T, T2> action)
		{
			int i = 0;
			foreach (var a in data)
			{
				yield return action(i, a);
				++i;
			}
		}
		public static IEnumerable<string> StringFormat<T>(this IEnumerable<T> data, string format)
		{
			foreach (var a in data)
			{
				yield return string.Format(format, a);
			}
		}

		public static string StringJoin<T>(this IEnumerable<T> data, string separator)
		{
			return string.Join(separator, data);
		}

		#endregion
	}
}