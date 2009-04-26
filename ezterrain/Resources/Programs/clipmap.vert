uniform sampler2D noise;

uniform float texScale;
uniform float texOffset;
uniform float heightScale;

void main()
{
	gl_TexCoord[0] = (gl_Vertex + texOffset) * texScale;
	
	vec4 vertex = gl_Vertex;
	
	vertex.z = heightScale * tex2D(noise, gl_TexCoord[0].st,8).r;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
