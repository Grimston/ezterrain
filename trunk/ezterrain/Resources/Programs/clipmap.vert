uniform float time;
uniform sampler2D noise;

varying float bias;

void main()
{
	vec4 vertex = gl_Vertex;
	bias = tex2D(noise, gl_TexCoord[0].st);
	
	vertex.z = bias;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
