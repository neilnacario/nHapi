/// <summary> The contents of this file are subject to the Mozilla Public License Version 1.1
/// (the "License"); you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at http://www.mozilla.org/MPL/
/// Software distributed under the License is distributed on an "AS IS" basis,
/// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for the
/// specific language governing rights and limitations under the License.
/// 
/// The Original Code is "SegmentGenerator.java".  Description:
/// "This class is responsible for generating source code for HL7 segment objects"
/// 
/// The Initial Developer of the Original Code is University Health Network. Copyright (C)
/// 2001.  All Rights Reserved.
/// 
/// Contributor(s):  Eric Poiseau. 
/// 
/// Alternatively, the contents of this file may be used under the terms of the
/// GNU General Public License (the  “GPL”), in which case the provisions of the GPL are
/// applicable instead of those above.  If you wish to allow use of your version of this
/// file only under the terms of the GPL and not to allow others to use your version
/// of this file under the MPL, indicate your decision by deleting  the provisions above
/// and replace  them with the notice and other provisions required by the GPL License.
/// If you do not delete the provisions above, a recipient may use your version of
/// this file under either the MPL or the GPL.
/// 
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using NHapi.Base;
using NHapi.Base.DataProvider;
using NHapi.Base.Log;

namespace NHapi.Base.SourceGeneration
{
	/// <summary> This class is responsible for generating source code for HL7 segment objects.
	/// Each automatically generated segment inherits from AbstractSegment.
	/// 
	/// </summary>
	/// <author>  Bryan Tripp (bryan_tripp@sourceforge.net)
	/// </author>
	/// <author>  Eric Poiseau
	/// </author>
	public class SegmentGenerator : Object
	{
		private static readonly IHapiLog log;

		/// <summary> <p>Creates skeletal source code (without correct data structure but no business
		/// logic) for all segments found in the normative database.  </p>
		/// </summary>
		public static void makeAll(String baseDirectory, String version)
		{
			//make base directory
			if (!(baseDirectory.EndsWith("\\") || baseDirectory.EndsWith("/")))
			{
				baseDirectory = baseDirectory + "/";
			}
			FileInfo targetDir =
				SourceGenerator.makeDirectory(baseDirectory + PackageManager.GetVersionPackagePath(version) + "Segment");

			var segmentSource = DataProviderFactory.Instance.GetProvider<ISegmentProvider>(log);
			ArrayList rawSegments = segmentSource.GetSegmentNames(version);
			ArrayList segments = new ArrayList();
			for (int i = 0; i < rawSegments.Count; i++)
			{
				segments.Add(altSegName((String)rawSegments[i]));
			}

			for (int i = 0; i < segments.Count; i++)
			{
				try
				{
					String seg = (String)segments[i];
					String source = makeSegment(seg, version);
					using (StreamWriter w = new StreamWriter(targetDir.ToString() + @"\" + GetSpecialFilename(seg) + ".cs"))
					{
						w.Write(source);
						w.Write("}");
					}
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("Error creating source code for all segments: " + e.Message);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
		}

		/// <summary>
		/// There are certain filenames that are reserved in windows.  CON is one of them.
		/// </summary>
		/// <param name="seg"></param>
		/// <returns></returns>
		private static string GetSpecialFilename(string seg)
		{
			if (seg.Equals("CON"))
				return "CON1";
			return seg;
		}

		/// <summary> <p>Returns an alternate segment name to replace the given segment name.  Substitutions
		/// made include:  </p>
		/// <ul><li>Replacing Z.. with Z</li>
		/// <li>Replacing ??? with ???</li></ul>
		/// </summary>
		public static String altSegName(String segmentName)
		{
			String ret = segmentName;
			if (ret.Equals("Z.."))
				ret = "Z";
			return ret;
		}

		/// <summary> Returns the Java source code for a class that represents the specified segment.</summary>
		public static String makeSegment(String name, String version)
		{
			Console.WriteLine("Making segment " + name);
			StringBuilder source = new StringBuilder();
			try
			{
				ArrayList elements;
				SegmentElement se;
				string segDesc;
				var segmentSource = DataProviderFactory.Instance.GetProvider<ISegmentProvider>(log);
				segmentSource.GetSegmentDefinition(name, version, out elements, out segDesc);

				//write imports, class documentation, etc ...
				source.Append("using System;\r\n");
				source.Append("using NHapi.Base;\r\n");
				source.Append("using NHapi.Base.Parser;\r\n");
				source.Append("using NHapi.Base.Model;\r\n");
				source.Append("using ");
				source.Append(PackageManager.GetVersionPackageName(version));
				source.Append("Datatype;\r\n");
				source.Append("using NHapi.Base.Log;\r\n\r\n");

				source.Append("namespace ");
				source.Append(PackageManager.GetVersionPackageName(version));
				source.Append("Segment{\r\n\r\n");
				source.Append("///<summary>\r\n");
				source.Append("/// Represents an HL7 ");
				source.Append(name);
				source.Append(" message segment. \r\n");
				source.Append("/// This segment has the following fields:<ol>\r\n");

				PrepareAppendStringsForElementsWithDuplicateDescriptions(name, elements);

				for (int i = 0; i < elements.Count; i++)
				{
					se = (SegmentElement)elements[i];
					source.Append("///");
					source.Append("<li>");
					source.Append(name);
					source.Append("-");
					source.Append(se.field);
					source.Append(": ");
					source.Append(se.GetDescriptionWithoutSpecialCharacters());
					source.Append(" (");
					source.Append(se.type);
					source.Append(")</li>\r\n");
				}
				source.Append("///</ol>\r\n");
				source.Append("/// The get...() methods return data from individual fields.  These methods \r\n");
				source.Append("/// do not throw exceptions and may therefore have to handle exceptions internally.  \r\n");
				source.Append("/// If an exception is handled internally, it is logged and null is returned.  \r\n");
				source.Append("/// This is not expected to happen - if it does happen this indicates not so much \r\n");
				source.Append("/// an exceptional circumstance as a bug in the code for this class.\r\n");
				source.Append("///</summary>\r\n");
				source.Append("[Serializable]\r\n");
				source.Append("public class ");
				source.Append(name);
				source.Append(" : AbstractSegment ");

				//implement interface from Model.control package if required
				/*Class correspondingControlInterface = Control.getInterfaceImplementedBy(name);
								if (correspondingControlInterface != null) {
								source.append("implements ");
								source.append(correspondingControlInterface.getName());
								} */

				source.Append(" {\r\n\r\n");
				source.Append("  /**\r\n");
				source.Append("   * Creates a ");
				source.Append(name);
				source.Append(" (");
				source.Append(segDesc);
				source.Append(") segment object that belongs to the given \r\n");
				source.Append("   * message.  \r\n");
				source.Append("   */\r\n");

				//write constructor
				source.Append("\tpublic ");
				source.Append(name);
				source.Append("(IGroup parent, IModelClassFactory factory) : base(parent,factory) {\r\n");
				source.Append("\tIMessage message = Message;\r\n");
				if (elements.Count > 0)
				{
					source.Append("    try {\r\n");
					for (int i = 0; i < elements.Count; i++)
					{
						se = (SegmentElement)elements[i];
						String type = SourceGenerator.getAlternateType(se.type, version);
						source.Append("       this.add(");
						source.Append("typeof(" + type + ")");
						//                    if (type.equalsIgnoreCase("Varies")) {
						//                    } else {
						//                        source.append("factory.getTypeClass(\"");
						//                        source.append(type);
						//                        source.append("\", \"");
						//                        source.append(version);
						//                        source.append("\")");
						//                    }
						source.Append(", ");
						if (se.opt == null)
						{
							source.Append("false");
						}
						else
						{
							if (se.opt.ToUpper().Equals("R".ToUpper()))
							{
								source.Append("true");
							}
							else
							{
								source.Append("false");
							}
						}
						source.Append(", ");
						source.Append(se.repetitions);
						source.Append(", ");
						source.Append(se.length);
						source.Append(", ");
						if (se.type.Equals("ID") || se.type.Equals("IS"))
						{
							source.Append("new System.Object[]{message, ");
							source.Append(se.table);
							source.Append("}");
						}
						else
						{
							source.Append("new System.Object[]{message}");
						}
						if (se.desc != null && se.desc.Trim().Length > 0)
						{
							source.Append(", ");


							source.Append("\"" + se.GetDescriptionWithoutSpecialCharacters() + "\"");
						}
						source.Append(");\r\n");
					}
					source.Append("    } catch (HL7Exception he) {\r\n");
					source.Append(
						"        HapiLogFactory.GetHapiLog(GetType()).Error(\"Can't instantiate \" + GetType().Name, he);\r\n");
					source.Append("    }\r\n");
				}
				source.Append("  }\r\n\r\n");

				//write a datatype-specific accessor for each field
				for (int i = 0; i < elements.Count; i++)
				{
					se = (SegmentElement)elements[i];
					if (!se.desc.ToUpper().Equals("UNUSED".ToUpper()))
					{
						//some entries in 2.1 DB say "unused"
						String type = SourceGenerator.getAlternateType(se.type, version);
						source.Append("\t///<summary>\r\n");
						source.Append("\t/// Returns ");
						if (se.repetitions != 1)
							source.Append("a single repetition of ");
						source.Append(se.GetDescriptionWithoutSpecialCharacters());
						source.Append("(");
						source.Append(name);
						source.Append("-");
						source.Append(se.field);
						source.Append(").\r\n");
						if (se.repetitions != 1)
						{
							source.Append("\t/// throws HL7Exception if the repetition number is invalid.\r\n");
							source.Append("\t/// <param name=\"rep\">The repetition number (this is a repeating field)</param>\r\n");
						}
						source.Append("\t///</summary>\r\n");
						source.Append("\tpublic ");
						source.Append(type);
						source.Append(" ");
						source.Append(SourceGenerator.MakeAccessorName(se.desc, se.repetitions) + se.AccessorNameToAppend);
						if (se.repetitions != 1)
							source.Append("(int rep)");
						source.Append("\n\t{\r\n");
						if (se.repetitions == 1)
							source.Append("\t\tget{\r\n");
						source.Append("\t\t\t");
						source.Append(type);
						source.Append(" ret = null;\r\n");
						source.Append("\t\t\ttry\n\t\t\t{\r\n");
						source.Append("\t\t\tIType t = this.GetField(");
						source.Append(se.field);
						source.Append(", ");
						if (se.repetitions == 1)
						{
							source.Append("0");
						}
						else
						{
							source.Append("rep");
						}
						source.Append(");\r\n");
						source.Append("\t\t\t\tret = (");
						source.Append(type);
						source.Append(")t;\r\n");
						if (se.repetitions == 1)
						{
							source.Append("\t\t\t}\n\t\t\t catch (HL7Exception he) {\r\n");
							source.Append(
								"\t\t\tHapiLogFactory.GetHapiLog(GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", he);\r\n");
							source.Append("\t\t\t\tthrow new System.Exception(\"An unexpected error ocurred\", he);\r\n");
						}
						source.Append("\t\t} catch (System.Exception ex) {\r\n");
						source.Append(
							"\t\t\tHapiLogFactory.GetHapiLog(GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", ex);\r\n");
						source.Append("\t\t\t\tthrow new System.Exception(\"An unexpected error ocurred\", ex);\r\n");
						source.Append("    }\r\n");
						source.Append("\t\t\treturn ret;\r\n");
						if (se.repetitions == 1)
							source.Append("\t}\r\n"); //End get
						source.Append("  }\r\n\r\n");


						//add an array accessor as well for repeating fields
						if (se.repetitions != 1)
						{
							source.Append("  ///<summary>\r\n");
							source.Append("  /// Returns all repetitions of ");
							source.Append(se.GetDescriptionWithoutSpecialCharacters());
							source.Append(" (");
							source.Append(name);
							source.Append("-");
							source.Append(se.field);
							source.Append(").\r\n");
							source.Append("   ///</summary>\r\n");
							source.Append("  public ");
							source.Append(type);
							source.Append("[] Get");
							source.Append(SourceGenerator.MakeAccessorName(se.desc) + se.AccessorNameToAppend);
							source.Append("() {\r\n");
							source.Append("     ");
							source.Append(type);
							source.Append("[] ret = null;\r\n");
							source.Append("    try {\r\n");
							source.Append("        IType[] t = this.GetField(");
							source.Append(se.field);
							source.Append(");  \r\n");
							source.Append("        ret = new ");
							source.Append(type);
							source.Append("[t.Length];\r\n");
							source.Append("        for (int i = 0; i < ret.Length; i++) {\r\n");
							source.Append("            ret[i] = (");
							source.Append(type);
							source.Append(")t[i];\r\n");
							source.Append("        }\r\n");
							source.Append("    } catch (HL7Exception he) {\r\n");
							source.Append(
								"        HapiLogFactory.GetHapiLog(this.GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", he);\r\n");
							source.Append("        throw new System.Exception(\"An unexpected error ocurred\", he);\r\n");
							source.Append("    } catch (System.Exception cce) {\r\n");
							source.Append(
								"        HapiLogFactory.GetHapiLog(GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", cce);\r\n");
							source.Append("        throw new System.Exception(\"An unexpected error ocurred\", cce);\r\n");
							source.Append("  }\r\n");
							source.Append(" return ret;\r\n");
							source.Append("}\r\n\r\n");

							//Add property for the total repetitions of this object
							source.Append("  ///<summary>\r\n");
							source.Append("  /// Returns the total repetitions of ");
							source.Append(se.GetDescriptionWithoutSpecialCharacters());
							source.Append(" (");
							source.Append(name);
							source.Append("-");
							source.Append(se.field);
							source.Append(").\r\n");
							source.Append("   ///</summary>\r\n");
							source.Append("  public int ");
							source.Append(SourceGenerator.MakeName(se.desc) + se.AccessorNameToAppend);
							source.Append("RepetitionsUsed\r\n");
							source.Append("{\r\n");
							source.Append("get{\r\n");
							source.Append("    try {\r\n");
							source.Append("\treturn GetTotalFieldRepetitionsUsed(" + se.field + ");\r\n");
							source.Append("    }\r\n");
							source.Append("catch (HL7Exception he) {\r\n");
							source.Append(
								"        HapiLogFactory.GetHapiLog(this.GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", he);\r\n");
							source.Append("        throw new System.Exception(\"An unexpected error ocurred\", he);\r\n");
							source.Append("} catch (System.Exception cce) {\r\n");
							source.Append(
								"        HapiLogFactory.GetHapiLog(GetType()).Error(\"Unexpected problem obtaining field value.  This is a bug.\", cce);\r\n");
							source.Append("        throw new System.Exception(\"An unexpected error ocurred\", cce);\r\n");
							source.Append("}\r\n");
							source.Append("}\r\n");
							source.Append("}\r\n");
						}
					}
				}

				//add adapter method code for control package if it exists
				//source.append(Control.getImplementation(correspondingControlInterface, version));

				source.Append("\n}");
			}
			catch (OleDbException sqle)
			{
				SupportClass.WriteStackTrace(sqle, Console.Error);
			}

			return source.ToString();
		}


		private static void PrepareAppendStringsForElementsWithDuplicateDescriptions(string name, ArrayList elements)
		{
			var segmentElements = elements.Cast<SegmentElement>().ToArray();
			var toProcess = new List<SegmentElement>();
			foreach (var segmentElement in segmentElements)
			{
				bool duplicateField =
					segmentElements.Count(
						element =>
							element.GetDescriptionWithoutSpecialCharacters() == segmentElement.GetDescriptionWithoutSpecialCharacters()) > 1;
				if (duplicateField)
				{
					toProcess.Add(segmentElement);
				}
			}

			foreach (var element in toProcess)
			{
				string toAppend = "_" + name + element.field;
				element.AccessorNameToAppend += toAppend;
			}
		}

		/// <summary>
		/// Main class
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		public static void Main(String[] args)
		{
			if (args.Length != 1 && args.Length != 2)
			{
				Console.Out.WriteLine("Usage: SegmentGenerator target_dir [segment_name]");
				Environment.Exit(1);
			}
			try
			{
				Type.GetType("sun.jdbc.odbc.JdbcOdbcDriver");
				if (args.Length == 1)
				{
					makeAll(args[0], "2.4");
				}
				else
				{
					String source = makeSegment(args[1], "2.4");
					StreamWriter w =
						new StreamWriter(new StreamWriter(args[0] + "/" + args[1] + ".java", false, Encoding.Default).BaseStream,
							new StreamWriter(args[0] + "/" + args[1] + ".java", false, Encoding.Default).Encoding);
					w.Write(source);
					w.Flush();
					w.Close();
				}
			}
			catch (Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}

		static SegmentGenerator()
		{
			log = HapiLogFactory.GetHapiLog(typeof(SegmentGenerator));
		}
	}
}