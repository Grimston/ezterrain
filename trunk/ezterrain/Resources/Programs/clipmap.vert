uniform sampler2D noise;

uniform float texScale;
uniform float texOffset;
uniform float heightScale;

varying vec3 normal;

vec4 calcTexCoord(vec4 vertex)
{
	return (vertex + texOffset) * texScale;
}

float getHeight(vec4 texCoord)
{
	return heightScale * tex2D(noise, texCoord.st).r;
}

void main()
{
	vec4 vertex = gl_Vertex;

	gl_TexCoord[0] = calcTexCoord(vertex);
	vertex.z = getHeight(gl_TexCoord[0]);
	
	vec4 komsu1 = gl_Vertex;
	//TODO: check bounds
	komsu1.x = komsu1.x + 1;
	komsu1.z = getHeight(calcTexCoord(komsu1));
	vec3 delta1 = normalize((komsu1 - vertex).xyz);
	
	vec4 komsu2 = gl_Vertex;
	//TODO: check bounds
	komsu2.y = komsu2.y + 1;
	komsu2.z = getHeight(calcTexCoord(komsu2));
	vec3 delta2 = normalize((komsu2 - vertex).xyz);
	
	vec4 komsu3 = gl_Vertex;
	//TODO: check bounds
	komsu3.x = komsu3.x - 1;
	komsu3.z = getHeight(calcTexCoord(komsu3));
	vec3 delta3 = normalize((komsu3 - vertex).xyz);
	
	vec4 komsu4 = gl_Vertex;
	//TODO: check bounds
	komsu4.y = komsu4.y - 1;
	komsu4.z = getHeight(calcTexCoord(komsu4));
	vec3 delta4 = normalize((komsu4 - vertex).xyz);

	vec3 normal1 = cross(delta1, delta2);
	vec3 normal2 = cross(delta2, delta3);
	vec3 normal3 = cross(delta3, delta4);
	vec3 normal4 = cross(delta4, delta1);
	normal = (normal1 + normal2 + normal3 + normal4) / 4.0;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
