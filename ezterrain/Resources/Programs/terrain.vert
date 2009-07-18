//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform float texScale;
uniform float outer;

const float level = outer + gl_InstanceID;
const float levelScale = pow(2.0, level);

in vec4 glVertex;

varying out vec2 texCoord;

void main()
{
	texCoord = glVertex.xy * texScale + 0.5;

	vec4 vertex = glVertex;
	vertex.xy = vertex.xy * levelScale;

	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
