using System;
using System.Xml;

namespace WixBuild
{
	public class WixComponentGroup : WixTag
	{
		public static readonly string PREFIX = "cmpgrp";

		public WixComponentGroup(string id) : base("ComponentGroup")
		{
			Id = id;
		}

		public string Id
		{
			get
			{
				return GetTag().GetAttribute("Id");
			}
			set
			{
				AddAttribute("Id", WixUtil.GenWixID(PREFIX, value));
			}
		}

		public void AddComponent(WixComponent component)
		{
			AddTag(component);
		}

		public void AddComponentRef(WixComponentRef component_ref)
		{
			AddTag(component_ref);
		}

		public WixComponentGroupRef ComponentGroupRef
		{
			get
			{
				return new WixComponentGroupRef(Id);
			}
		}
	}
}
