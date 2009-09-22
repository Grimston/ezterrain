//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform sampler2DArray heightMaps;

uniform float outer;

varying in float level;

varying in vec2 texCoord;

void main()
{
	gl_FragColor = texture2DArray(heightMaps, vec3(texCoord, level)) / 2000.0;
}
