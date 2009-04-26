uniform float texScale;
uniform float texOffset;

void main()
{
	gl_TexCoord[0] = (gl_Vertex + texOffset) * texScale;
	gl_Position = ftransform();
}
