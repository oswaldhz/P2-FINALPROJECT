import { useEffect, useState } from 'react';
import {
  Box,
  Button,
  Container,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Input,
  SimpleGrid,
  Stack,
  Table,
  Tbody,
  Td,
  Text,
  Th,
  Thead,
  Tr,
  useToast,
} from '@chakra-ui/react';
import { fetchReservas, login, register, fetchEquipos, createReserva } from './api';

function App() {
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('user');
    return saved ? JSON.parse(saved) : null;
  });
  const [authMode, setAuthMode] = useState('login');
  const [form, setForm] = useState({ email: '', password: '', nombre: '' });
  const [reservas, setReservas] = useState([]);
  const [equipos, setEquipos] = useState([]);
  const [reservaForm, setReservaForm] = useState({ equipoId: '', fechaInicio: '', fechaFin: '', notas: '' });
  const toast = useToast();

  useEffect(() => {
    if (token) {
      loadReservas();
      loadEquipos();
    }
  }, [token]);

  const loadReservas = async () => {
    try {
      const data = await fetchReservas();
      setReservas(data);
    } catch (err) {
      toast({ status: 'error', title: 'No se pudieron cargar las reservas' });
    }
  };

  const loadEquipos = async () => {
    try {
      const data = await fetchEquipos();
      setEquipos(data);
    } catch (err) {
      toast({ status: 'error', title: 'No se pudieron cargar los equipos' });
    }
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response =
        authMode === 'login'
          ? await login(form.email, form.password)
          : await register(form.email, form.nombre, form.password);

      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response));
      setToken(response.token);
      setUser(response);
    } catch (err) {
      toast({ status: 'error', title: 'Credenciales inválidas o usuario existente' });
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
    setReservas([]);
  };

  const handleReserva = async (e) => {
    e.preventDefault();
    try {
      await createReserva({
        equipoId: reservaForm.equipoId,
        fechaInicio: reservaForm.fechaInicio,
        fechaFin: reservaForm.fechaFin,
        notas: reservaForm.notas,
      });
      toast({ status: 'success', title: 'Reserva creada' });
      setReservaForm({ equipoId: '', fechaInicio: '', fechaFin: '', notas: '' });
      loadReservas();
    } catch (err) {
      toast({ status: 'error', title: 'No se pudo crear la reserva' });
    }
  };

  if (!token) {
    return (
      <Container maxW="md" py={10}>
        <Heading mb={6}>{authMode === 'login' ? 'Inicia sesión' : 'Regístrate'}</Heading>
        <Box as="form" onSubmit={handleLogin} p={6} borderWidth="1px" borderRadius="lg" boxShadow="md">
          <Stack spacing={4}>
            {authMode === 'register' && (
              <FormControl isRequired>
                <FormLabel>Nombre</FormLabel>
                <Input
                  value={form.nombre}
                  onChange={(e) => setForm({ ...form, nombre: e.target.value })}
                  placeholder="Tu nombre"
                />
              </FormControl>
            )}
            <FormControl isRequired>
              <FormLabel>Correo</FormLabel>
              <Input
                type="email"
                value={form.email}
                onChange={(e) => setForm({ ...form, email: e.target.value })}
                placeholder="correo@ejemplo.com"
              />
            </FormControl>
            <FormControl isRequired>
              <FormLabel>Contraseña</FormLabel>
              <Input
                type="password"
                value={form.password}
                onChange={(e) => setForm({ ...form, password: e.target.value })}
                placeholder="********"
              />
            </FormControl>
            <Button type="submit" colorScheme="teal">
              {authMode === 'login' ? 'Ingresar' : 'Crear cuenta'}
            </Button>
            <Button variant="link" onClick={() => setAuthMode(authMode === 'login' ? 'register' : 'login')}>
              {authMode === 'login' ? '¿No tienes cuenta? Regístrate' : '¿Ya tienes cuenta? Inicia sesión'}
            </Button>
          </Stack>
        </Box>
      </Container>
    );
  }

  return (
    <Container maxW="6xl" py={8}>
      <Flex justify="space-between" align="center" mb={6}>
        <Heading size="lg">Reservas</Heading>
        <Stack direction="row" spacing={4} align="center">
          <Text>{user?.email}</Text>
          <Button colorScheme="red" variant="outline" onClick={handleLogout}>
            Cerrar sesión
          </Button>
        </Stack>
      </Flex>

      <Box mb={8} p={6} borderWidth="1px" borderRadius="lg" boxShadow="sm">
        <Heading size="md" mb={4}>
          Crear reserva
        </Heading>
        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={4} as="form" onSubmit={handleReserva}>
          <FormControl isRequired>
            <FormLabel>Equipo</FormLabel>
            <Input
              list="equipos"
              placeholder="Selecciona un equipo"
              value={reservaForm.equipoId}
              onChange={(e) => setReservaForm({ ...reservaForm, equipoId: e.target.value })}
            />
            <datalist id="equipos">
              {equipos.map((eq) => (
                <option key={eq.id} value={eq.id}>
                  {eq.nombre}
                </option>
              ))}
            </datalist>
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Inicio</FormLabel>
            <Input
              type="datetime-local"
              value={reservaForm.fechaInicio}
              onChange={(e) => setReservaForm({ ...reservaForm, fechaInicio: e.target.value })}
            />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Fin</FormLabel>
            <Input
              type="datetime-local"
              value={reservaForm.fechaFin}
              onChange={(e) => setReservaForm({ ...reservaForm, fechaFin: e.target.value })}
            />
          </FormControl>
          <FormControl>
            <FormLabel>Notas</FormLabel>
            <Input
              value={reservaForm.notas}
              onChange={(e) => setReservaForm({ ...reservaForm, notas: e.target.value })}
            />
          </FormControl>
          <Button type="submit" colorScheme="teal">
            Guardar
          </Button>
        </SimpleGrid>
      </Box>

      <Box borderWidth="1px" borderRadius="lg" boxShadow="sm" overflowX="auto">
        <Table variant="simple">
          <Thead>
            <Tr>
              <Th>Equipo</Th>
              <Th>Usuario</Th>
              <Th>Inicio</Th>
              <Th>Fin</Th>
              <Th>Estado</Th>
            </Tr>
          </Thead>
          <Tbody>
            {reservas.map((reserva) => (
              <Tr key={reserva.id}>
                <Td>{reserva.equipo}</Td>
                <Td>{reserva.usuario}</Td>
                <Td>{new Date(reserva.fechaInicio).toLocaleString()}</Td>
                <Td>{new Date(reserva.fechaFin).toLocaleString()}</Td>
                <Td>{reserva.estado}</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Box>
    </Container>
  );
}

export default App;
