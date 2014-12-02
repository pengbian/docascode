<?xml version="1.0"?>
<xsl:stylesheet
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	exclude-result-prefixes="msxsl"
	xmlns="http://ddue.schemas.microsoft.com/authoring/2003/5"
	version="1.1">

	<xsl:output method="xml" indent="yes" encoding="UTF-8" />

	<xsl:template match="summary">
		<summary>
			<xsl:call-template name="HandleBlocks">
				<xsl:with-param name="blockdata" select="para|list|code" />
				<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
			</xsl:call-template>        
		</summary>
	</xsl:template>

	<xsl:template match="exception">
		<exception>
			<codeEntityReference autoUpgrade="true">
				<xsl:value-of select="@cref" />
			</codeEntityReference>
			<content>
				<xsl:call-template name="HandleBlocks">
					<xsl:with-param name="blockdata" select="para|list|code" />
					<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
				</xsl:call-template>         
			</content>
		</exception>
	</xsl:template>

	<xsl:template match="param">
		<parameter>
			<parameterReference>
				<xsl:value-of select="@name" />
			</parameterReference>
			<xsl:call-template name="HandleBlocks">
				<xsl:with-param name="blockdata" select="para|list|code" />
				<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
			</xsl:call-template>       
		</parameter>
	</xsl:template>

	<xsl:template match="typeparam">
		<genericParameter>
			<parameterReference>
				<xsl:value-of select="@name" />
			</parameterReference>
			<xsl:call-template name="HandleBlocks">
				<xsl:with-param name="blockdata" select="para|list|code" />
				<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
			</xsl:call-template>       
		</genericParameter>
	</xsl:template>

	<xsl:template match="returns|value">
		<returnValue>
			<xsl:call-template name="HandleBlocks">
				<xsl:with-param name="blockdata" select="para|list|code" />
				<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
			</xsl:call-template>      
		</returnValue>
	</xsl:template>
	
	<xsl:template match="b">
		<legacyBold>
			<xsl:apply-templates />
		</legacyBold>
	</xsl:template>

	<xsl:template match="i">
		<legacyItalic>
			<xsl:apply-templates />
		</legacyItalic>
	</xsl:template>
	
	<xsl:template match="languageKeyword">
		<languageKeyword>
			<xsl:apply-templates />
		</languageKeyword>
	</xsl:template>

	<xsl:template match="token">
		<token>
			<xsl:apply-templates />
		</token>
	</xsl:template>

	<xsl:template match="ui">
		<ui>
			<xsl:apply-templates />
		</ui>
	</xsl:template>
	
	<xsl:template match="c">
		<codeInline>
			<xsl:apply-templates />
		</codeInline>
	</xsl:template>

	<xsl:template match="code">
		<code>
			<xsl:apply-templates select="@*|node()"/>
		</code>
	</xsl:template>

	<xsl:template match="list">
		<list>
			<xsl:attribute name="class">
				<xsl:value-of select="@type"/>
			</xsl:attribute>
			<xsl:apply-templates />
		</list>
	</xsl:template>

	<xsl:template match="listItem">
		<listItem>
			<xsl:call-template name="HandleBlocks">
				<xsl:with-param name="blockdata" select="para|list|code" />
				<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
			</xsl:call-template>
		</listItem>
	</xsl:template>  
	
	<xsl:template match="example">
		<codeExample>
			<xsl:call-template name="HandleCode" />
		</codeExample>
	</xsl:template>

	<xsl:template name="HandleCode">
		<xsl:choose>
			<xsl:when test="boolean(code)">
				<xsl:call-template name="HandleDescription">
					<xsl:with-param name="current" select="code[1]" />
				</xsl:call-template>
				<xsl:apply-templates select="code" />
				<xsl:call-template name="HandleComments" >
					<xsl:with-param name="current" select="code[last()]" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="HandleDescription">
		<xsl:param name ="current" />
		<xsl:variable name="blockdata" select="$current/preceding-sibling::para|$current/preceding-sibling::list" />
		<xsl:variable name="data" select="$current/preceding-sibling::node()[not(self::text()[not(normalize-space())])]" />
		<xsl:if test="count($data)">
			<description>
				<content>
					<xsl:call-template name="HandleBlocks">
						<xsl:with-param name="blockdata" select="$blockdata" />
						<xsl:with-param name="data" select="$data" />
					</xsl:call-template>
				</content>
			</description>
		</xsl:if>
	</xsl:template>

	<xsl:template name="HandleComments">
		<xsl:param name ="current" />
		<xsl:variable name="blockdata" select="$current/following-sibling::para|$current/following-sibling::list" />
		<xsl:variable name="data" select="$current/following-sibling::node()[not(self::text()[not(normalize-space())])]" />
		<xsl:if test="count($data)">
			<comments>
				<content>
					<xsl:call-template name="HandleBlocks">
						<xsl:with-param name="blockdata" select="$blockdata" />
						<xsl:with-param name="data" select="$data" />
					</xsl:call-template>
				</content>
			</comments>
		</xsl:if>
	</xsl:template>

	<xsl:template match="remarks">
		<remarks>
			<content>
				<xsl:call-template name="HandleBlocks">
					<xsl:with-param name="blockdata" select="para|list|code" />
					<xsl:with-param name="data" select="node()[not(self::text()[not(normalize-space())])]" />
				</xsl:call-template>
			</content>
		</remarks>
	</xsl:template>
	
	<xsl:template name="HandleBlocks">
		<xsl:param name="blockdata"/>
		<xsl:param name="data" />
		<xsl:choose>
			<xsl:when test="count($blockdata)">
				<xsl:call-template name="HandlePreBlock">
					<xsl:with-param name="current" select="$blockdata[1]"/>
					<xsl:with-param name="datalimit" select="$data" />
				</xsl:call-template>
				<xsl:for-each select="$blockdata">         
						<xsl:apply-templates select="."/>
					<xsl:call-template name="HandlePostBlock">
						<xsl:with-param name="datalimit" select="$data" />
					</xsl:call-template>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="count($data)">
				<para>
					<xsl:apply-templates select="$data"/>
				</para>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="HandlePreBlock">
		<xsl:param name="current"/>
		<xsl:param name="datalimit" />
		<xsl:variable name="seq" select="$current/preceding-sibling::node()[not(self::text()[not(normalize-space())]) and count(.|$datalimit)=count($datalimit)]"/>
		<xsl:if test="count($seq)">
			<para>
				<xsl:apply-templates select="$seq"/>
			</para>
		</xsl:if>
	</xsl:template>

	<xsl:template name="HandlePostBlock">
		<xsl:param name="datalimit" />
		<xsl:variable name="next" select="(following-sibling::para|following-sibling::list|following-sibling::code)[count(.|$datalimit)=count($datalimit)]"/>
		<xsl:choose>
			<xsl:when test="boolean($next)">
				<xsl:variable name="currentIndex" select="count(preceding-sibling::node())+1"/>
				<xsl:variable name="seq1" select="($next[1]/preceding-sibling::node())[position()>$currentIndex and not(self::text()[not(normalize-space())])]"/>
				<xsl:if test="boolean($seq1)">
					<para>
						<xsl:apply-templates select="$seq1"/>
					</para>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="seq2" select="following-sibling::node()[not(self::text()[not(normalize-space())]) and count(.|$datalimit)=count($datalimit)]"/>
				<xsl:if test="boolean($seq2)">
					<para>
						<xsl:apply-templates select="$seq2"/>
					</para>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="para">
		<para>
			<xsl:apply-templates select="node()"/>
		</para>
	</xsl:template>

	<xsl:template match="paramref">
		<parameterReference>
				<xsl:value-of select="@name" />
		</parameterReference>
	</xsl:template>

	<xsl:template match="typeparamref">
		<parameterReference>
			<xsl:value-of select="@name" />
		</parameterReference>
	</xsl:template>
	
	<xsl:template match="see">
		<codeEntityReference autoUpgrade="true">
				<xsl:value-of select="@cref" />
		</codeEntityReference>
	</xsl:template>

	<xsl:template match="seealso">
		<relatedTopics>
			<codeEntityReference autoUpgrade="true">
				<xsl:value-of select="@cref" />
			</codeEntityReference>
		</relatedTopics>
	</xsl:template>
	
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>